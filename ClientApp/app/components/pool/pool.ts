import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { App } from '../app/app';
import { global, MessageStatus, IPool, IPoolRanking, IRank, IMessage } from '../../services/globals'
import * as moment from 'moment';

@autoinject()
export class Pool {
  private _http: HttpClient;
  public pool: IPool;
  public message: IMessage;
  public ranking: IRank[];

  constructor(http: HttpClient) {
      this._http = http;
  }

  public activate() {
    this._http.fetch('/api/Pool')
        .then(result => result.json() as Promise<IPool>)
        .then(data => {
            this.pool = data;
        });

    this._http.fetch('/api/Pool/Ranking')
        .then(result => result.json() as Promise<IPoolRanking>)
        .then(data => {
            this.ranking = data.ranking;
        });

  }

  public selectMessage() {
    this.message = null;
  }

  public edit(id: string) {
    location.href = `/Users/Edit/${id}`;
  }

  public post() {
    if (this.message) {
      this.message.message = this.message.message ?  this.message.message : 'Awesome!'
      let post = { message: this.message.message, userId: this.pool.user.user.id };

      this._http.fetch('/api/pool', {
          method: 'post',
          body: json(post)
      })
      .then(result => result.json() as Promise<IPool>)
      .then(data => {
          this.pool = data;
          this.message = null;
          global.toastr('Message send');
      });
    }
  }

  public approveMessage(message: IMessage) {
    message.status = (message.status != MessageStatus.Approved) ? MessageStatus.Approved : MessageStatus.Rejected;
    let post = { id: message.id, status: message.status, userId: this.pool.user.user.id };
    
    this._http.fetch('/api/pool', {
      method: 'post',
      body: json(post)
    })
    .then(result => result.json() as Promise<IPool>)
    .then(data => {
      this.pool = data;
      this.message = null;
      global.toastr('Message status updated');
    });
  }

  public canManage(roles: string[]): boolean {
    if (roles === null) { return false; }
    for  (var role of roles){
      if (role === 'Admin') {
        return true;
      }
    }

    return false;
  }

  public formatDate(value: string): string {
    const DATEFORMAT: string = 'D MMMM YYYY HH:mm';
    const date: moment.Moment = moment.utc(value);

    if (date.isValid()) {
      return date.local().format(DATEFORMAT);
    }

    return value;
  }
}