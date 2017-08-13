import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { MeetingModel } from './../../../../models/meetingModel';

@Component({
  selector: 'app-meeting-options',
  templateUrl: './meetingoptions.component.html',
  styleUrls: ['./meetingoptions.component.css']
})
export class MeetingOptionsComponent implements OnInit {
  MeetingKey: string = 'meeting';
  IsLoggedIn: boolean;
  Meeting: MeetingModel;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
    this.Meeting = JSON.parse(localStorage.getItem('meeting'));
  }

  ngOnInit() {
    
  }

  save(){
    console.log(JSON.parse(localStorage.getItem(this.MeetingKey)));
  }
}