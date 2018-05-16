import { autoinject, bindable } from 'aurelia-framework';
import { global, ISchedule, ICountry } from '../../../services/globals'

@autoinject()
export class PoolSchedule {
  @bindable public country: ICountry;
  @bindable public items: ISchedule[] = [];

  public formatDate(value: string, format: string): string {
    return global.formatDate(value, format);
  }

  public result(schedule: ISchedule): string {
    if (schedule.goals1 === -1 || schedule.goals2 === -1) {
      return '';
    }

    return `${ schedule.goals1 } - ${ schedule.goals2 }`;
  }

  public imageUrl(code: string) {
    return global.imageUrl(code);
  }

  public selectSchedule(schedule: ISchedule) {
    global.toastr(schedule.matchId);
  }
}