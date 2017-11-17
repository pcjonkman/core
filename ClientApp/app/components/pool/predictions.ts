import { HttpClient } from 'aurelia-fetch-client';
import { autoinject, computedFrom } from 'aurelia-framework';
import { BindingSignaler } from 'aurelia-templating-resources';
import { global, ICountry, IPoolCountry, IPoolPrediction } from '../../services/globals'

interface IEventArguments {
  id?: string;
}

@autoinject()
export class Predictions {
  private _bindingSignaler: BindingSignaler;
  private _http: HttpClient;
  public countries: ICountry[];
  public pool: IPoolPrediction;

  constructor(bindingSignaler: BindingSignaler, http: HttpClient) {
    this._bindingSignaler = bindingSignaler;
    this._http = http;
  }

  public activate(args: IEventArguments) {
    const id = args.id ? args.id : ''
    this._http.fetch(`/api/Pool/Country`)
        .then(result => result.json() as Promise<IPoolCountry>)
        .then(data => {
            this.countries = data.country;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        })
        .catch((errorMessage: string) => {
          this.countries = undefined;
          global.toastr(errorMessage, true);
        });
    this._http.fetch(`/api/Pool/Prediction/${id}`)
        .then(result => result.json() as Promise<IPoolPrediction>)
        .then(data => {
            this.pool = data;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        })
        .catch((errorMessage: string) => {
          this.pool = { user: null, match: undefined, finals: undefined };
          global.toastr(errorMessage, true);
        });
  }
 }