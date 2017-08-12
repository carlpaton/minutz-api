import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-meeting-tabzone',
  templateUrl: './meetingtabzone.component.html',
  styleUrls: ['./meetingtabzone.component.css']
})
export class MeetingTabZoneComponent implements OnInit {
  IsLoggedIn: boolean;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
    
  }

}