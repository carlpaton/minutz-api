import { Component, OnInit } from '@angular/core';
import { AuthService } from './../auth/auth.service';
import { HomeTasksComponent } from './hometask/hometasks.component';
import { HomeMeetingsComponent } from './homemeetings/homemeetings.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  IsLoggedIn: boolean;
  constructor(public auth: AuthService) {

  }

  ngOnInit() {
  }

}
