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
    selector: 'sd-timezone',
    templateUrl: 'timezone.component.html',
    styleUrls: ['timezone.component.css']
  })
  export class TimeZoneComponent implements AfterViewInit {
    Id: string = '';
    @Input() Date: string;
    @Output() Change = new EventEmitter<any>();
    public ngAfterViewInit() {
      $('#' + this.Id).betterTimezone({
        showIANA: false,
        showOptgroups: false // cannot be true when showIANA is false 
      });
    }
    public SelectedDate(time:any) {
      this.Change.emit(time);
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
  