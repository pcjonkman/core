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
  private pool: IPoolSchedule;
  private country: ICountry[];
  private group: string = 'A';
  
  constructor(bindingSignaler: BindingSignaler, http: HttpClient) {
    this._bindingSignaler = bindingSignaler;
    this._http = http;
  }

  public activate() {
    this._http.fetch(`/api/Pool/Country`)
        .then(result => result.json() as Promise<IPoolCountry>)
        .then(data => {
            this.country = data.country; //.filter((country: ICountry) => { return country.group === this.group; });
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        });
    this._http.fetch('/api/Pool/Schedule')
        .then(result => result.json() as Promise<IPoolSchedule>)
        .then(data => {
            this.pool = data;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        });    
  }

  public selectSchedule(schedule: ISchedule) {
    global.toastr(schedule.matchId);
  }

  public formatDate(value: string, format: string): string {
    const date: moment.Moment = moment.utc(value);

    if (date.isValid()) {
      return date.local().format(format);
    }

    return value;
  }

  public score(schedule: ISchedule): string {
    if (schedule.goals1 === -1 || schedule.goals2 === -1) {
      return '';
    }

    return `${ schedule.goals1 } - ${ schedule.goals2 }`;
  }

  public goals(poule: IPoule): string {
    return `${ poule.plus - poule.min > 0 ? '+' : '' }${ poule.plus - poule.min }`;
  }

  public imageUrl(code: string) {
    if (code === null || code === undefined) { return ''; }
    return require(`../../../../node_modules/flag-icon-css/flags/4x3/${ code.toLowerCase() }.svg`);
  }

  @computedFrom('pool', 'country')
  public get groups(): string[] {
    if (this.country === undefined) { return; }
    const group = this.country.map((country: ICountry) => { return country.group; });
    return Array.from(new Set(group));
  }

  @computedFrom('pool', 'country')
  public get countries(): ICountry[] {
    return this.country;
  }

  // @computedFrom('pool')
  // public get scheduleCountry(): ISchedule[] {
  //   const countryId = 1;
  public scheduleCountry(countryId: number): ISchedule[] {
    const schedule: ISchedule[] = this.pool ? this.pool.schedule.filter((item: ISchedule) => { return item.country1Id === countryId || item.country2Id === countryId }) : [];
    
    return schedule;
  }

  // @computedFrom('pool', 'country')
  // public get scheduleGroup(): ISchedule[] {
  public scheduleGroup(group: string): ISchedule[] {
    const schedule: ISchedule[] = this.pool ? this.pool.schedule.filter((item: ISchedule) => { return item.group === group; }) : [];

    return schedule;
  }

  // @computedFrom('country', 'pool')
  // public get schedulePoule(): IPoule[] {
  public schedulePoule(group: string): IPoule[] {
    if (!this.pool && !this.country) { return; }
    const schedule: ISchedule[] = this.pool ? this.pool.schedule.filter((item: ISchedule) => { return item.group === group; }) : [];
    const poule: IPoule[] = this.country ? this.country.filter((country: ICountry) => { return country.group === group; }).map((item: ICountry) => {
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