import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-meeting-options',
  templateUrl: './meetingoptions.component.html',
  styleUrls: ['./meetingoptions.component.css']
})
export class MeetingOptionsComponent implements OnInit {
  IsLoggedIn: boolean;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
    
  }

}