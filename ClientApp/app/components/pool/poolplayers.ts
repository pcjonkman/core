import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import * as moment from 'moment';
import { App } from '../app/app';
import { global, IPoolPoolPlayer, IPoolPlayer } from '../../services/globals'

@autoinject()
export class PoolPlayers {
  private _http: HttpClient;
  private pool: IPoolPoolPlayer;

  constructor(http: HttpClient) {
      this._http = http;
  }

  public activate() {
    this._http.fetch('/api/Pool/PoolPlayers')
    .then(result => result.json() as Promise<IPoolPoolPlayer>)
    .then(data => {
        this.pool = data;
    });

  }

  public selectPoolPlayer(poolPlayer: IPoolPlayer) {
    global.toastr(poolPlayer.name);
  }
}