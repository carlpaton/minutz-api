import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../auth/auth.service';

@Component({
  selector: 'app-hometasks',
  templateUrl: './hometasks.component.html',
  styleUrls: ['./hometasks.component.css']
})
export class HomeTasksComponent implements OnInit {
  IsLoggedIn: boolean;
  constructor(public auth: AuthService) {

  }

  ngOnInit() {
  }

}