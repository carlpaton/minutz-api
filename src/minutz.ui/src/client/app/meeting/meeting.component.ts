import {
  Component,
  OnInit
} from '@angular/core';
import { ActivatedRoute } from '@angular/router'
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
  ngOnInit() {
    this.route.params.subscribe(params => {
      this.Id = params["id"];
    });
  
  }
  constructor(
    private route: ActivatedRoute
  ) {
    this.Check = true;
    this.TestDate = '08/02/2016';
   }

  saveModel(): void {
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
  testEvent($event: any): void {

    console.log($event);
  }

  save(): void {

  }

  create() {

  }

  discover() {
    console.log('discovery');
  }

  enableSearch() {

  }

  search(): void {

  }
}
