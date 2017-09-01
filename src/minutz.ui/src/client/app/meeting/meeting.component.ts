import {
  Component,
  OnInit
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
declare let $: any;
@Component({
  moduleId: module.id,
  selector: 'sd-meeting',
  templateUrl: 'meeting.component.html',
  styleUrls: ['meeting.component.css']
})
export class MeetingComponent implements OnInit {
  Name = 'Meeting';
  Id: string = '';
  Check: boolean;
  TestDate: string;
  public ngOnInit() {
    this.route.params.subscribe(params => {
      this.Id = params['id'];
    });
  }
  public constructor(
    private route: ActivatedRoute
  ) {
    this.Check = true;
    this.TestDate = '08/02/2016';
   }
  public saveModel(): void {
    console.log('save');
  }
  public TestClick($evnet: any){
    this.TestDate = '08/02/2015';
   //if(this.Check){ this.Check = false;}else{this.Check = true;} 
  }
  public CheckChangedEvent($event: any){
    console.log($event);
  }
  public SerachByOptionChange($event: any): void {
    console.log($event);
  };
  public testEvent($event: any): void {
    console.log($event);
  }
  public save(): void {
    console.log('save');
  }
  public create() {
    console.log('create');
  }
  public discover() {
    console.log('discovery');
  }
  public enableSearch() {
    console.log('discovery');
  }
  search(): void {
    console.log('discovery');
  }
}
