import * as toastr from 'toastr';

export var global = {
  toastr: function toast(msg) {
    toastr.success(`${msg}`);
  },
  imageUrl: function(code: string) {
    if (code === null || code === undefined) { return ''; }
    return require(`../../../node_modules/flag-icon-css/flags/4x3/${ code.toLowerCase() }.svg`);
  }
}

export enum MessageStatus {
  Submitted = 0,
  Approved = 1,
  Rejected = 2
}

export interface IMessage {
  id: number;
  name: string;
  placedDate: string;
  message: string;
  status: number;
}

export interface IPool {
  user: IUser|null;
  messages: IMessage[];
}

export interface IPoolPlayer {
  id: number;
  name: string;
  subScore: string;
  openQuestions: string;
}

export interface IPoolPoolPlayer {
  user: IUser|null;
  poolplayers: IPoolPlayer[];
}

export interface IRank {
  rank: number;
  name: string;
  score: number;
}

export interface IPoolCountry {
  user: IUser|null;
  country: ICountry[];
}

export interface IPoolRanking {
  user: IUser|null;
  ranking: IRank[];
}

export interface IPoolSchedule {
  user: IUser|null;
  schedule: ISchedule[]
}

export interface ISchedule {
  matchId: number;
  group: string;
  startDate: string;
  country1: string;
  country2: string;
  country1Code: string;
  country2Code: string;
  country1Id: number;
  country2Id: number;
  goals1: number;
  goals2: number;
}

export interface ICountry {
  id: number;
  name: string;
  code: string;
  group: string;
}

export interface IPoule {
  countryId: number;
  country: string;
  countryCode: string;
  play: number;
  win: number;
  draw: number;
  lost: number;
  point: number;
  plus: number;
  min: number;
}

export interface IUser {
  firstName: string;
  lastName: string;
  isLoggedIn: boolean;
  lastLoginDate: string;
  user: any;
  roles: string[];
}
