import { HttpClient } from 'aurelia-fetch-client';
import { autoinject, computedFrom } from 'aurelia-framework';
import { BindingSignaler } from 'aurelia-templating-resources';
import { global, ICountry, IPoolCountry, IPoolResults } from '../../services/globals'

interface IEventArguments {
  id?: string;
}

@autoinject()
export class Results {
  private _bindingSignaler: BindingSignaler;
  private _http: HttpClient;
  public countries: ICountry[];
  public pool: IPoolResults;

  constructor(bindingSignaler: BindingSignaler, http: HttpClient) {
    this._bindingSignaler = bindingSignaler;
    this._http = http;
  }

  public activate() {
    const id = '';
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
    this._http.fetch(`/api/Pool/Results`)
        .then(result => result.json() as Promise<IPoolResults>)
        .then(data => {
            this.pool = data;
            window.setTimeout(() => { this._bindingSignaler.signal('data'); }, 0);
        })
        .catch((errorMessage: string) => {
          this.pool = { user: null, poolPlayer: undefined, schedule: undefined };
          global.toastr(errorMessage, true);
        });
  }
 }