import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit, Input } from '@angular/core';
import { MeetingComponent } from './../../meeting.component';
import { MeetingModel } from './../../../../models/meetingModel';

@Component({
  selector: 'app-meeting-heading',
  templateUrl: './meetingheading.component.html',
  styleUrls: ['./meetingheading.component.css']
})
export class MeetingHeadingComponent implements OnInit {
  MeetingKey: string = 'meeting';
  IsLoggedIn: boolean;
  MeetingHeading: string;
  Meeting: MeetingModel;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
    var session = JSON.parse(localStorage.getItem(this.MeetingKey));
    this.Meeting = session;
    this.MeetingHeading = session.Name;
  }

  ngOnInit() {
    
  }

  textChange($event){
    var session = JSON.parse(localStorage.getItem(this.MeetingKey));
    session.Name = $event;
    localStorage.setItem(this.MeetingKey, JSON.stringify(session));
  }

}