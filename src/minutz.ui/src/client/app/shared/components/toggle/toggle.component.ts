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
    selector: 'sd-toggle',
    templateUrl: 'toggle.component.html',
    styleUrls: ['toggle.component.css']
  })
  export class ToggleComponent implements AfterViewInit, OnInit {
    Name: string;
    @Input() On:string;
    @Input() Off: string;
    @Output() Change = new EventEmitter<any>();
    public ngAfterViewInit() {
        $('#' + this.Name).bootstrapToggle({ on: this.On, off: this.Off });
    }
    public ngOnInit(){
        this.Name = 'toggle-' + this.createId();
        if(this.On){this.On = this.On;}else{this.On = 'On';}
        if(this.Off){this.Off = this.Off;}else{this.Off = 'Off';}
    }
    public SelectedDate(time:any) {
      this.Change.emit(time);
    }
    private createId(): any {
      return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
        `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
      return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
  }
  