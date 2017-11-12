import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject, computedFrom } from 'aurelia-framework';
import { BindingSignaler } from 'aurelia-templating-resources';
import * as moment from 'moment';
import { App } from '../app/app';
import { global, IPoolSchedule, ISchedule, IPoule, IPoolCountry, ICountry } from '../../services/globals'

@autoinject()
export class Schedule {
  private _bindingSignaler: BindingSignaler;
  private _http: HttpClient;
  private _pool: IPoolSchedule;
  private _countries: ICountry[];

  constructor(bindingSignaler: BindingSignaler, http: HttpClient) {
    this._bindingSignaler = bindingSignaler;
    this._http = http;
  }

  public activate() {
    this._http.fetch(`/api/Pool/Country`)
        .then(result => result.json() as Promise<IPoolCountry>)
        .then(data => {
            this._countries = data.country;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        })
        .catch((errorMessage: string) => {
          this._countries = undefined;
          global.toastr(errorMessage, true);
        });
    this._http.fetch('/api/Pool/Schedule')
        .then(result => result.json() as Promise<IPoolSchedule>)
        .then(data => {
            this._pool = data;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        })
        .catch((errorMessage: string) => {
          this._pool = { user: null, schedule: undefined };
          global.toastr(errorMessage, true);
        });
  }

  public score(poule: IPoule): string {
    return `${ poule.plus - poule.min > 0 ? '+' : '' }${ poule.plus - poule.min }`;
  }

  public imageUrl(code: string) {
    return global.imageUrl(code);
  }

  @computedFrom('_pool', '_countries')
  public get groups(): string[] {
    if (this._countries === undefined) { return; }
    const group = this._countries.map((country: ICountry) => { return country.group; });
    return Array.from(new Set(group));
  }

  @computedFrom('_pool', '_country')
  public get countries(): ICountry[] {
    return this._countries;
  }

  @computedFrom('_pool', '_country')
  public get pool(): IPoolSchedule {
    return this._pool;
  }

  public scheduleCountry(countryId: number): ISchedule[] {
    const schedule: ISchedule[] = this._pool ? this._pool.schedule.filter((item: ISchedule) => { return item.country1Id === countryId || item.country2Id === countryId }) : [];

    return schedule;
  }

  public scheduleGroup(group: string): ISchedule[] {
    const schedule: ISchedule[] = this._pool ? this._pool.schedule.filter((item: ISchedule) => { return item.group === group; }) : [];

    return schedule;
  }

  public schedulePoule(group: string): IPoule[] {
    if (!this._pool && !this._countries) { return; }
    const schedule: ISchedule[] = this._pool ? this._pool.schedule.filter((item: ISchedule) => { return item.group === group; }) : [];
    const poule: IPoule[] = this._countries ? this._countries.filter((country: ICountry) => { return country.group === group; }).map((item: ICountry) => {
      return {
        countryId: item.id,
        country: item.name,
        countryCode: item.code,
        play: 0,
        win: 0,
        draw: 0,
        lost: 0,
        point: 0,
        min: 0,
        plus: 0
      } as IPoule
    }) : [];
    poule.forEach((poule: IPoule) => {
      schedule.forEach((schedule: ISchedule) => {
        if ((schedule.country1Id === poule.countryId || schedule.country2Id === poule.countryId) &&
            (schedule.goals1 !== -1 && schedule.goals2 !== -1)) {
          poule.play++;
          if (schedule.country1Id === poule.countryId) { // home
            poule.plus += schedule.goals1;
            poule.min += schedule.goals2;
            if (schedule.goals1 > schedule.goals2) {
              poule.win++;
              poule.point += 3;
            }
            if (schedule.goals1 < schedule.goals2) {
              poule.lost++;
            }
            if (schedule.goals1 == schedule.goals2) {
              poule.draw++;
              poule.point += 1;
            }
          }
          if (schedule.country2Id === poule.countryId) { //away
            poule.plus += schedule.goals2;
            poule.min += schedule.goals1;
            if (schedule.goals1 > schedule.goals2) {
              poule.lost++;
            }
            if (schedule.goals1 < schedule.goals2) {
              poule.win++;
              poule.point += 3;
            }
            if (schedule.goals1 == schedule.goals2) {
              poule.draw++;
              poule.point += 1;
            }
          }
        }
      });
    });
    poule.sort((a: IPoule, b: IPoule) => {
      if (a.point === b.point) {
        if ((a.plus-a.min) === (b.plus-a.min)) {
          if (a.plus === b.plus) {
            return a.country < b.country ? -1 : 1; // sort by name
          }
          return -(a.plus - b.plus); // sort by most scored goals
        }
        return -((a.plus-a.min) - (b.plus-b.min)); // sort by score
      }
      return -(a.point - b.point); // sort by points
    });

    return poule;
  }
 }