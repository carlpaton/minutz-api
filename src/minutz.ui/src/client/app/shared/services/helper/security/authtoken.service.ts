import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { AuthenticationParams } from '../../../config/authenticationParams';
import { UrlService } from '../url/url.service';
import { LocalStorageService, BaseHttpComponent } from '../../index';

@Injectable()
export class AuthenticationTokenService {
  public token: any = {};
  public user: any = {};
  private timer: Observable<number>;
  private jwtTokenName = 'auth.jwt.token';
  private userInfo = 'user.information';

  constructor(private _http: BaseHttpComponent,
    private _urlService: UrlService,
    private _localStorageService: LocalStorageService, ) {
  }

  public isValid(token: any): boolean {
    // Get current epoch timestamp
    let currentUtcDate = Math.round(new Date().getTime() / 1000.0);
    let expiryDate = Date.parse(this._localStorageService.get(this.jwtTokenName + '.expiry'));

    // Make sure we have a user to check expiration on
    this.token = token;
    this.user = localStorage.getItem(this.userInfo);
    if (this.token) {
      if (currentUtcDate <= expiryDate) {
        return true;
      }
    }
    return false;
  }

  public retrieveTokenFromExternalAuth(): void {
    window.location.href = AuthenticationParams.externalLoginPage;
  }

  public GetAccessToken(code: string) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    this._http.post(
      `${AuthenticationParams.tokenApiEndpoint}`,
      `grant_type=authorization_code&` +
      `redirect_uri=${AuthenticationParams.tokenApiRedirectUrl}&` +
      `client_id=${AuthenticationParams.client_id}&` +
      `client_secret=${AuthenticationParams.client_secret}&` +
      `code=${code}`,
      { headers: headers, withCredentials: true }
    ).subscribe(response => {
      let tokenBody = response.json();
      localStorage.setItem(this.jwtTokenName, JSON.stringify(tokenBody));
      this._decodeJWT(tokenBody.access_token);
      this.setExpiryToken(tokenBody.expires_in);
      this.startTimer();
      this._urlService.removeTokenFromUrl();
    });
  }
  public getUserToken(): any {
    return this._localStorageService.get(this.jwtTokenName);
  }

  public getUserTokenExpiry(): any {
    let expiry = this._localStorageService.getRaw(this.jwtTokenName + '.expiry');
    return new Date(expiry);
  }

  public startTimer() {
    if (this.timer === null || this.timer === undefined) {
      this.timer = Observable.timer(0, 10000);
      this.timer.subscribe(t => this.Observer(t));
    }
  }

  public getUserInfo(): any {
    let item = localStorage.getItem(this.userInfo);
    if (item) {
      return JSON.parse(localStorage.getItem(this.userInfo));
    } else {
      return {};
    }
  }

  private Observer(tick: number) {
    let testMode = false;//set this to true to refresh token constantly
    let safeExpiry = new Date(new Date().getTime() - 20000);
    if (safeExpiry > this.getUserTokenExpiry() || testMode) {
      this.getRefreshToken();
    }
  }

  private getRefreshToken() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    this._http.post(
      `${AuthenticationParams.tokenApiEndpoint}`,
      `grant_type=refresh_token&` +
      `client_id=${AuthenticationParams.client_id}&` +
      `client_secret=${AuthenticationParams.client_secret}&` +
      `refresh_token=${JSON.parse(localStorage.getItem(this.jwtTokenName)).refresh_token}`,
      { headers: headers }
    ).subscribe(response => {
      let tokenBody = response.json();
      localStorage.setItem(this.jwtTokenName, JSON.stringify(tokenBody));
      this.setExpiryToken(tokenBody.expires_in);
    }, error => {
      localStorage.removeItem(this.jwtTokenName);
    });
  }

  private setExpiryToken(expires_in: number) {
    let timeObject = new Date();
    let expiry = new Date(timeObject.getTime() + expires_in * 1000);
    localStorage.setItem(this.jwtTokenName + '.expiry', JSON.stringify(expiry.toString()));
  }

  private padRight(width: number, str: string, padding: string): string {
    return (width <= str.length) ? str : this.padRight(width, str + padding, padding);
  }

  private _decodeJWT(token: string): void {
    let base64Url: any = token.split('.')[1];
    let base64: any = base64Url.replace('-', '+').replace('_', '/');
    localStorage.setItem(this.userInfo, window.atob(base64));
  }
}
