import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject, BindingEngine, bindable, computedFrom } from 'aurelia-framework';
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules } from 'aurelia-validation';
import { BindingSignaler } from 'aurelia-templating-resources';
import { Subscription } from 'aurelia-event-aggregator';
import * as moment from 'moment';
import { MomentInput } from 'moment';
import { global, IPoolPrediction, IMatchPrediction, IFinalsPrediction, ICountry } from '../../../services/globals';
import { BootstrapFormRenderer } from '../../../services/bootstrapFormRenderer';

@autoinject()
export class PoolPrediction {
  @bindable public country: ICountry;
  @bindable public pool: IPoolPrediction;
  @bindable public countries: ICountry[] = [];
  public countries1: ICountry[] = [];
  public countries2: ICountry[] = [];
  public countries4: ICountry[] = [];
  public countries8: ICountry[] = [];
  public countries16: ICountry[] = [];
  public selectedCountries1: ICountry[] = [];
  public selectedCountries2: ICountry[] = [];
  public selectedCountries4: ICountry[] = [];
  public selectedCountries8: ICountry[] = [];
  public selectedCountries16: ICountry[] = [];
  
  public predictionRules: ValidationRules;

  private _closingDate: string = "2017-12-19T10:39:00"

  private _bindingEngine: BindingEngine;
  private _bindingSignaler: BindingSignaler;
  private _http: HttpClient;
  private _subscriptions: Subscription[] = [];
  public controller: ValidationController;

  constructor(bindingEngine: BindingEngine, bindingSignaler: BindingSignaler, http: HttpClient, private validator: Validator, controllerFactory: ValidationControllerFactory) {
    this._bindingEngine = bindingEngine;
    this._bindingSignaler = bindingSignaler;
    this._http = http;
    this.controller = controllerFactory.createForCurrentScope(validator);
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.setupValidation();
  }

  public attached() {
    this._subscriptions.push(this._bindingEngine.propertyObserver(this, 'countries').subscribe(() => {
      this.countries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
      this.countries1 = this.countries.slice(0);
      this.countries2 = this.countries.slice(0);
      this.countries4 = this.countries.slice(0);
      this.countries8 = this.countries.slice(0);
      this.countries16 = this.countries.slice(0);
      window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
    }));
    this._subscriptions.push(this._bindingEngine.propertyObserver(this, 'pool').subscribe(() => {
      this.selectedCountries16 = [];
      this.selectedCountries8 = [];
      this.selectedCountries4 = [];
      this.selectedCountries2 = [];
      this.selectedCountries1 = [];
      this.countries1 = this.countries.slice(0);
      this.countries2 = this.countries.slice(0);
      this.countries4 = this.countries.slice(0);
      this.countries8 = this.countries.slice(0);
      this.countries16 = this.countries.slice(0);
      this.pool.finals.forEach((fp: IFinalsPrediction) => {
        switch(fp.level) {
          case 1:
            this.selectedCountries16.push(this.countries.find((c: ICountry) => { return c.id === fp.countryId; }));
            this.countries16.splice(this.countries16.findIndex((c: ICountry) => { return c.id === fp.countryId; }), 1);
            break;
          case 2:
            this.selectedCountries8.push(this.countries.find((c: ICountry) => { return c.id === fp.countryId; }));
            this.countries8.splice(this.countries8.findIndex((c: ICountry) => { return c.id === fp.countryId; }), 1);
            break;
          case 3:
            this.selectedCountries4.push(this.countries.find((c: ICountry) => { return c.id === fp.countryId; }));
            this.countries4.splice(this.countries4.findIndex((c: ICountry) => { return c.id === fp.countryId; }), 1);
            break;
          case 4:
            this.selectedCountries2.push(this.countries.find((c: ICountry) => { return c.id === fp.countryId; }));
            this.countries2.splice(this.countries2.findIndex((c: ICountry) => { return c.id === fp.countryId; }), 1);
            break;
          case 5:
            this.selectedCountries1.push(this.countries.find((c: ICountry) => { return c.id === fp.countryId; }));
            this.countries1.splice(this.countries1.findIndex((c: ICountry) => { return c.id === fp.countryId; }), 1);
            break;
        }
      });
      window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
    }));
  }

  public detached(): void {
    this._subscriptions.forEach((subscription: Subscription) => { subscription.dispose(); });
    this._subscriptions = [];
  }

  private setupValidation() {
    ValidationRules.customRule(
      'integerRange',
      (value, obj, min, max) => {
        var num = Number.parseInt(value);
        return num === null || num === undefined || (Number.isInteger(num) && num >= min && num <= max);
      },
      "${$displayName} must be an integer between ${$config.min} and ${$config.max}.",
      (min, max) => ({ min, max })
    );

    this.predictionRules = ValidationRules
    .ensure('predictedGoals1')
      .maxLength(1)
        .when((mp: IMatchPrediction) => mp.predictedGoals1 !== -1)
      .satisfiesRule('integerRange', 0, 9)
        .when((mp: IMatchPrediction) => mp.predictedGoals1 !== -1)
    .ensure('predictedGoals2')
      .maxLength(1)
        .when((mp: IMatchPrediction) => mp.predictedGoals2 !== -1)
      .satisfiesRule('integerRange', 0, 9)
        .when((mp: IMatchPrediction) => mp.predictedGoals2 !== -1)
    .rules;
  }

  @computedFrom('pool')
  public get match() {
    return this.pool.match;
    // return this.pool.match.filter((item: IMatchPrediction) => {
    //   return item.group === 'A' || 
    //   item.group ==='B' || 
    //   item.group ==='C' || 
    //   item.group ==='D' || 
    //   item.group ==='E' || 
    //   item.group ==='F';
    // })
  }

  @computedFrom('pool', 'controller', 'isClosed')
  public get isDisabled(): boolean {
    return (!this.pool.user.isLoggedIn || this.controller.errors.length !== 0 || this.isClosed) ? true : false;
  }

  @computedFrom('_closingDate')
  public get isClosed(): boolean {
    const now: moment.Moment = moment.utc();
    const closingDate: moment.Moment = this._closingDate === undefined ? now : moment.utc(this._closingDate as MomentInput);
    return now.isSameOrAfter(closingDate)
  }

  @computedFrom('_closingDate')
  public get closingDate(): string {
    return moment.utc(this._closingDate).local().format('DD-MM-YYYY hh:mm');
  }

  public cancelBubbling(event: Event): boolean {
    event.stopPropagation();

    return true;
  }

  public formatDate(value: string, format: string): string {
    const date: moment.Moment = moment.utc(value);

    if (date.isValid()) {
      return date.local().format(format);
    }

    return value;
  }

  public result(schedule: IMatchPrediction): string {
    if (schedule.goals1 === -1 || schedule.goals2 === -1) {
      return '';
    }

    return `${ schedule.goals1 } - ${ schedule.goals2 }`;
  }

  public score(schedule: IMatchPrediction): number {
    return schedule.subScore;
  }

  public resultScore(schedule: IMatchPrediction): string {
    return `(${ this.result(schedule) }) - ${ this.score(schedule) }`;
  }

  public imageUrl(code: string) {
    return global.imageUrl(code);
  }

  public selectSchedule(schedule: IMatchPrediction) {
    global.toastr(schedule.matchId);
  }

  public post() {
    this.pool.finals = [];
    this.selectedCountries16.forEach((country: ICountry) => {
      this.pool.finals.push({ country: country.name, countryCode: country.code, countryId: country.id, level: 1, subScore: 0 });
    });
    this.selectedCountries8.forEach((country: ICountry) => {
      this.pool.finals.push({ country: country.name, countryCode: country.code, countryId: country.id, level: 2, subScore: 0 });
    });
    this.selectedCountries4.forEach((country: ICountry) => {
      this.pool.finals.push({ country: country.name, countryCode: country.code, countryId: country.id, level: 3, subScore: 0 });
    });
    this.selectedCountries2.forEach((country: ICountry) => {
      this.pool.finals.push({ country: country.name, countryCode: country.code, countryId: country.id, level: 4, subScore: 0 });
    });
    this.selectedCountries1.forEach((country: ICountry) => {
      this.pool.finals.push({ country: country.name, countryCode: country.code, countryId: country.id, level: 5, subScore: 0 });
    });
    this.controller.validate({ object: this.pool, propertyName: 'match', rules: this.predictionRules })
      .then(result => {
        if (result.valid) {
          this._http.fetch('/api/pool/prediction', {
              method: 'post',
              body: json(this.pool)
          })
          .then(result => result.json() as Promise<IPoolPrediction>)
          .then(data => {
              this.pool = data;
              window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
              global.toastr('Data send');
          });
        } else {
          global.toastr('Data is not valid', true);
        }
      });
  }
}