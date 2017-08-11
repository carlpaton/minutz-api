import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-meeting-heading',
  templateUrl: './meetingheading.component.html',
  styleUrls: ['./meetingheading.component.css']
})
export class MeetingHeadingComponent implements OnInit {
  IsLoggedIn: boolean;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
    
  }

}