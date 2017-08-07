import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../auth/auth.service';

@Component({
  selector: 'app-homemeetings',
  templateUrl: './homemeetings.component.html',
  styleUrls: ['./homemeetings.component.css']
})
export class HomeMeetingsComponent implements OnInit {
  IsLoggedIn: boolean;
  constructor(public auth: AuthService) {

  }

  ngOnInit() {
  }

}