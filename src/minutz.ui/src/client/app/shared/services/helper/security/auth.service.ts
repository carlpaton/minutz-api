import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Rx';

import {UrlService,AuthenticationTokenService,LocalStorageService} from '../../index';

@Injectable()
export class AuthenticationService {

  private jwtTokenName = 'auth.jwt.token';

  constructor(private _localStorageService: LocalStorageService,
              private _authenticationTokenService: AuthenticationTokenService,
              private _urlService: UrlService) { }

  public isAuthenticated(): boolean {
    let token = this._localStorageService.get(this.jwtTokenName);
    if (token) {
      return this._authenticationTokenService.isValid(token);
    }
    return false;
  }

   public authenticateUser(): Observable<any> {
     return Observable.create((observer: any) => {
      if (!this.isAuthenticated()) {
        if (!this.isTokenRedirect()) {
          this._authenticationTokenService.retrieveTokenFromExternalAuth();
          observer.next(null);
          observer.complete();
        }

        this.getTokenIdFromUrl();
      }});
  }

  private isTokenRedirect(): boolean {
    let query = window.location.search === '' ? window.location.hash  : window.location.search;
    let queryObject = this._urlService.convertQueryStringToJson(query);
    return queryObject && queryObject.code;
  }

  private getTokenIdFromUrl() {
    if (this.isTokenRedirect()) {
      let query = window.location.search === '' ? window.location.hash  : window.location.search;
      let queryObject = this._urlService.convertQueryStringToJson(query);
      this._authenticationTokenService.GetAccessToken(queryObject.code);
    }
  }
}
