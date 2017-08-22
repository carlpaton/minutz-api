import { Component, OnInit } from '@angular/core';
import { AuthenticationTokenService } from '../shared/services/index';

@Component({
  moduleId: module.id,
  selector: 'home',
  templateUrl: 'home.component.html'
})
export class HomeComponent implements OnInit {
  public userInfo: any;

  constructor(private _authenticationTokenService: AuthenticationTokenService) {
  }

  ngOnInit() {
    this.userInfo = this._authenticationTokenService.getUserInfo();
  }

}
