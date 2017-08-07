import { AuthService } from './auth/auth.service';
import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core';
//import * as jQuery from 'jquery';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {
  title = 'Minutz';
  IsLoggedIn: boolean;
  constructor(
    private elementRef: ElementRef,
    public auth: AuthService
  ) {
    if (!localStorage.getItem('access_token')) {
      auth.login();
    }
    auth.handleAuthentication();
    this.IsLoggedIn = auth.isAuthenticated();
  }
  ngAfterViewInit() {

  }
  public ngOnInit() {

  }

}
