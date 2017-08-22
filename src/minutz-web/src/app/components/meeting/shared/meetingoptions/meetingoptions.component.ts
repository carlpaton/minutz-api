import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

import { MeetingModel } from './../../../../models/meetingModel';
import { IMyDpOptions, IMyInputFieldChanged } from 'mydatepicker';

@Component({
  selector: 'app-meeting-options',
  templateUrl: './meetingoptions.component.html',
  styleUrls: ['./meetingoptions.component.css']
})
export class MeetingOptionsComponent implements OnInit {
  MeetingKey: string = 'meeting';
  IsLoggedIn: boolean;
  Meeting: MeetingModel;
  private myDatePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'dd.mm.yyyy',
  };
  private model: Object = { date: { year: 2018, month: 10, day: 9 } };

  constructor(public auth: AuthService) {
    this.IsLoggedIn = auth.isAuthenticated();
    this.Meeting = JSON.parse(localStorage.getItem('meeting'));
  }

  ngOnInit() {

  }

  locationChange($event) {
    var session = JSON.parse(localStorage.getItem(this.MeetingKey));
    session.Location = $event;
    localStorage.setItem(this.MeetingKey, JSON.stringify(session));
  }

  save() {
    console.log(JSON.parse(localStorage.getItem(this.MeetingKey)));
  }
}