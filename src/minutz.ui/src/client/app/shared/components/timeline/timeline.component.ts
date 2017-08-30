import {
    Component,
    OnInit
  } from '@angular/core';
  @Component({
    moduleId: module.id,
    selector: 'sd-timeline',
    templateUrl: 'timeline.component.html',
    styleUrls: ['timeline.component.css']
  })
  export class TimelineComponent implements OnInit {
    ngOnInit() {
      console.log('timeline init');
    }
    constructor(){
      console.log('timeline construct');
    }
  }
