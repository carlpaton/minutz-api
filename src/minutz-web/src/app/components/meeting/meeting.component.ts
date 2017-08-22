import { AuthService } from './../../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { CreateMeetingComponent } from './create/createmeeting.component';
import { EditMeetingComponent } from './edit/editmeeting.component';
import { RunMeetingComponent } from './run/runmeeting.component';
import { MeetingModel } from './../../models/meetingModel';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css']
})
export class MeetingComponent implements OnInit {
  IsLoggedIn: boolean;
  Mode: string;
  Meeting: MeetingModel;

  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
    this.Mode = 'create';

    if(this.Mode == 'create'){
      this.createmeetingObject();
    }

    
  }
  private createmeetingObject(){
    var meeting = localStorage.getItem('meeting');
    if(meeting){
      localStorage.removeItem('meeting');
    }
    
    this.Meeting = new MeetingModel();
    localStorage.setItem('meeting', JSON.stringify(this.Meeting));
  }
  
  ngOnInit() {
    
  }

}