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
    selector: 'sd-attendee-select',
    templateUrl: 'attendeeSelect.component.html',
    styleUrls: ['attendeeSelect.component.css']
  })
  export class AttendeeSelectComponent implements AfterViewInit {
    Attendees:Array<any> = [];
    Id: string = '';
    @Input() SetAttendee: string;
    @Output() Change = new EventEmitter<any>();
    public ngAfterViewInit() {
     console.log('get data');
     this.Attendees.push('Lee-Roy');
     this.Attendees.push('Mauro');
    }
    public SelectedDate(time:any) {
      this.Change.emit(time);
    }
    public SelectedAttendee() {
        console.log('');
    }
    constructor() {
      this.Id = 'time-zone-' + this.createId();
    }
    private createId(): any {
      return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
        `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
      return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
  }
  