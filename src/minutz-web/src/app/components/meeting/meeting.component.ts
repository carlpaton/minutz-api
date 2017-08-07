import { AuthService } from './../../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { CreateMeetingComponent } from './create/createmeeting.component';
import { EditMeetingComponent } from './edit/editmeeting.component';
import { RunMeetingComponent } from './run/runmeeting.component';

@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css']
})
export class MeetingComponent implements OnInit {
  IsLoggedIn: boolean;
  Mode: string;

  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
    this.Mode = 'create';
  }

  ngOnInit() {
    
  }

}