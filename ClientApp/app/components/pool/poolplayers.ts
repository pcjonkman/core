import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject, computedFrom } from 'aurelia-framework';
import * as moment from 'moment';
import { App } from '../app/app';
import { global, IPoolPoolPlayer, IPoolPlayer } from '../../services/globals'
import { Router } from 'aurelia-router';

@autoinject()
export class PoolPlayers {
  private _http: HttpClient;
  private _router: Router;
  public pool: IPoolPoolPlayer;

  constructor(http: HttpClient, router: Router) {
      this._http = http;
      this._router = router;
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