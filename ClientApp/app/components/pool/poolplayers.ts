import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject, computedFrom } from 'aurelia-framework';
import * as moment from 'moment';
import { App } from '../app/app';
import { global, IPoolPoolPlayer, IPoolPlayer } from '../../services/globals'
import { Router } from 'aurelia-router';

@autoinject()
export class PoolPlayers {
  public pool: IPoolPoolPlayer;

  constructor(private readonly _http: HttpClient, private readonly _router: Router) {
  }

  public activate() {
    this._http.fetch('/api/Pool/PoolPlayers')
    .then(result => result.json() as Promise<IPoolPoolPlayer>)
    .then(data => {
        this.pool = data;
    })
    .catch((errorMessage: string) => {
      this.pool = { user: null, poolPlayer: undefined, poolPlayers: undefined };
      global.toastr(errorMessage, true);
    });
  }

  public selectPoolPlayer(poolPlayer: IPoolPlayer) {
    // global.toastr(poolPlayer.name);
    this._router.navigate(`predictions/${poolPlayer.id}`)
  }
}