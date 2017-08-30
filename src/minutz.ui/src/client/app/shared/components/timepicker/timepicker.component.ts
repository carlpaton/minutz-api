import {
  Component,
  OnInit,
  Input,
  EventEmitter,
  Output,
  OnChanges,
  AfterViewInit
} from '@angular/core';
declare let $: any;
@Component({
  moduleId: module.id,
  selector: 'sd-timepicker',
  templateUrl: 'timepicker.component.html',
  styleUrls: ['timepicker.component.css']
})
export class TimePickerComponent implements AfterViewInit {
  Id: string = '';
  @Input() Date: string;
  @Output() Change = new EventEmitter<any>();
  public ngAfterViewInit() {
    $('#' + this.Id).timepicker();
    $('#' + this.Id).timepicker().on('changeTime.timepicker', function (e: any) {
      //console.log('The time is ' + e.time.value);
      //console.log('The hour is ' + e.time.hours);
      //console.log('The minute is ' + e.time.minutes);
      //console.log('The meridian is ' + e.time.meridian);
      this.Change.emit(e.time);
    });
  }
  constructor() {
    this.Id = 'time-picker-' + this.createId();
  }
  private createId(): any {
    return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
      `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
  }
  private createidsection() {
    return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
  }
}
