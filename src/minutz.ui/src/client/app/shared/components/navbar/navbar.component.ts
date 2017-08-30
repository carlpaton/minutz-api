import {
  Component,
  ElementRef,
  OnInit,
  Input,
  EventEmitter,
  Output,
  OnChanges,
  AfterViewInit } from '@angular/core';
declare let $: any;
@Component({
  moduleId: module.id,
  selector: 'navbar',
  templateUrl: 'navbar.component.html'
})
export class NavbarComponent implements AfterViewInit {
  constructor(private el: ElementRef) {
  }
  public ngAfterViewInit() {
    //$('#' + this.Name).select2();
  }
}
