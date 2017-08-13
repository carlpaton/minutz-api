import { AuthService } from './../../../auth/auth.service';
import { Component, OnInit, Input } from '@angular/core';
import { MeetingComponent } from './../meeting.component';
import { MeetingModel } from './../../../models/meetingModel';


@Component({
  selector: 'app-create-meeting',
  templateUrl: './createmeeting.component.html',
  styleUrls: ['./createmeeting.component.css']
})
export class CreateMeetingComponent implements OnInit {
  IsLoggedIn: boolean;
  Meeting: MeetingModel;

  constructor(public auth: AuthService) {
    this.IsLoggedIn = auth.isAuthenticated();
    this.Meeting = JSON.parse(localStorage.getItem('meeting'));
  }

  ngOnInit() {

  }

}