import { HttpClient, json } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules } from 'aurelia-validation';
import { App } from '../app/app';
import { global, MessageStatus, IPool, IPoolRanking, IRank, IMessage } from '../../services/globals'
import * as moment from 'moment';
import { Router } from 'aurelia-router';
import { BootstrapFormRenderer } from '../../services/bootstrapFormRenderer';

@autoinject()
export class Pool {
  public pool: IPool;
  public message: IMessage;
  public ranking: IRank[];
  public changeName: boolean = false;
  public controller: ValidationController;
  public nameRules: ValidationRules;
  
  constructor(private readonly _http: HttpClient, private readonly _router: Router, private validator: Validator, controllerFactory: ValidationControllerFactory) {
    this.controller = controllerFactory.createForCurrentScope(validator);
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.setupValidation();
  }

  private setupValidation() {
    this.nameRules = ValidationRules
    .ensure('name')
      .required()
      .minLength(3)
    .rules;
  }

  public activate() {
    this._http.fetch('/api/Pool')
        .then(result => result.json() as Promise<IPool>)
        .then(data => {
            this.pool = data;
        })
        .catch((errorMessage: string) => {
          this.pool = { user: null, poolPlayer: null, messages: undefined };
          global.toastr(errorMessage, true);
        });

    this._http.fetch('/api/Pool/Ranking')
        .then(result => result.json() as Promise<IPoolRanking>)
        .then(data => {
            this.ranking = data.ranking;
        })
        .catch((errorMessage: string) => {
          this.ranking = undefined;
          global.toastr(errorMessage, true);
        });

  }

  public cancelBubbling(event: Event): boolean {
    event.stopPropagation();

    return true;
  }

  public selectMessage() {
    this.message = null;
  }

  public edit(id: string) {
    location.href = `/Users/Edit/${id}`;
  }

  public editName() {
    this.changeName = !this.changeName;
  }

  public postName() {
    this.changeName = !this.changeName;

    this.controller.validate()
    .then(result => {
      if (result.valid) {
        this._http.fetch('/api/pool/PoolPlayer', {
          method: 'post',
          body: json(this.pool.poolPlayer)
        })
        .then(result => result.json() as Promise<IPool>)
        .then(data => {
          this.pool = data;
          this.message = null;
          global.toastr('Name updated');
        });
      } else {
        global.toastr('Name is not valid', true);
      }
    });
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
    return global.formatDate(value, DATEFORMAT);
  }

  public select(id: number) {
    // global.toastr(poolPlayer.name);
    this._router.navigate(`predictions/${id}`)
  }
}