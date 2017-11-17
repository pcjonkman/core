import { autoinject, BindingEngine, bindable, computedFrom } from 'aurelia-framework';
import { BindingSignaler } from 'aurelia-templating-resources';
import { Subscription } from 'aurelia-event-aggregator';
import { global, ICountry } from '../../../services/globals';

@autoinject()
export class PoolCountrySelector {
  @bindable public title: string;
  @bindable public countries: ICountry[];
  @bindable public selectedCountries: ICountry[];
  @bindable public max: number;
  @bindable public disable: boolean = false;
  
  public addCountries: ICountry[] = [];
  public removeCountries: ICountry[] = [];
  
  private _bindingEngine: BindingEngine;
  private _bindingSignaler: BindingSignaler;
  private _subscriptions: Subscription[] = [];

  private _show: boolean = false;
  
  constructor(bindingEngine: BindingEngine, bindingSignaler: BindingSignaler) {
    this._bindingEngine = bindingEngine;
    this._bindingSignaler = bindingSignaler;
  }

  public attached() {
    this._subscriptions.push(this._bindingEngine.propertyObserver(this, 'countries').subscribe(() => {
      this.countries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
    }));
  }

  public detached(): void {
    this._subscriptions.forEach((subscription: Subscription) => { subscription.dispose(); });
    this._subscriptions = [];
  }

  public select(countries: ICountry[], selectedCountries: ICountry[], max: number) {
    this.addCountries.forEach((country: ICountry) => {
      if (selectedCountries.length != max) {
        if (selectedCountries.indexOf(country) < 0) selectedCountries.push(country);
        countries.splice(countries.indexOf(country), 1);
      }
    });
    this.countries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
    this.selectedCountries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
    this.addCountries = [];
    window.setTimeout(() => { this._bindingSignaler.signal('selected'); }, 0);
  }

  public deselect(countries: ICountry[], selectedCountries: ICountry[]) {
    this.removeCountries.forEach((country: ICountry) => {
      if (countries.indexOf(country) < 0) countries.push(country);
      selectedCountries.splice(selectedCountries.indexOf(country), 1);
    });
    this.countries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
    this.selectedCountries.sort((a: ICountry, b: ICountry) => { return a.name > b.name ? 1 : -1; });
    this.removeCountries = [];
    window.setTimeout(() => { this._bindingSignaler.signal('selected'); }, 0);
  }

  public toSelect(countries: ICountry[], max: number) {
    return `${ this.max - countries.length } to select`;
  }

  public imageUrl(code: string) {
    return global.imageUrl(code);
  }

  public toggleShow() {
    this._show = !this._show;
  }

  @computedFrom('_show')
  public get show() {
    return this._show;
  }
}