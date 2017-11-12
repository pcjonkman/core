import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules } from 'aurelia-validation';
import * as moment from 'moment';
import { global, IPoolPrediction, IMatchPrediction, ICountry } from '../../../services/globals';
import { BootstrapFormRenderer } from '../../../services/bootstrapFormRenderer';

@autoinject()
export class PoolPrediction {
  @bindable public country: ICountry;
  @bindable public pool: IPoolPrediction;

  public predictionRules: ValidationRules;

  private _http: HttpClient;
  public controller: ValidationController;

  constructor(http: HttpClient, private validator: Validator, controllerFactory: ValidationControllerFactory) {
    this._http = http;
    this.controller = controllerFactory.createForCurrentScope(validator);
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.setupValidation();
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
    return this.pool.match.filter((item: IMatchPrediction) => {
      return item.group === 'A' || 
      item.group ==='B' || 
      item.group ==='C' || 
      item.group ==='D' || 
      item.group ==='E' || 
      item.group ==='F';
    })
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
    this.controller.validate()
      .then(result => {
        if (result.valid) {
          this._http.fetch('/api/pool/prediction', {
              method: 'post',
              body: json(this.pool)
          })
          .then(result => result.json() as Promise<IPoolPrediction>)
          .then(data => {
              this.pool = data;
              global.toastr('Data send');
          });
        } else {
          global.toastr('Data is not valid', true);
        }
      });
  }
}