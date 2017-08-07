import { AuthService } from '../../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { CalendarComponent } from './../shared/calendar/calendar.component';

@Component({
  selector: 'app-diary',
  templateUrl: './diary.component.html',
  styleUrls: ['./diary.component.css']
})
export class DiaryComponent implements OnInit {
   IsLoggedIn: boolean;
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
  }

}