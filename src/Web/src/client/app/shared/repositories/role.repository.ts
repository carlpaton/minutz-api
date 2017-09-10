import { Injectable } from '@angular/core';
import { Http, Response,  RequestMethod } from '@angular/http';
import { AuthHttpRequestService } from './../services/extensions/app.http.wrapper';
import { Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/Rx';
import { Role } from '../models/index';
@Injectable()
export class RoleRepository {
  constructor(private _http: AuthHttpRequestService) {
  }
  getAll(): Observable<Role[]> {
    let roles$ = this._http
      .processRequest(`api/roles`,RequestMethod.Get, null, null)
      .map((roleData: any) => {
        let response: Array<Role> = [];
        if (roleData) {
          JSON.parse(roleData._body).forEach((r: string) => {
            response.push(new Role(r));
          });
        }
        return response;
      })
      .catch(handleError);
      return roles$;
  }
}

function handleError (error: any) {
  let errorMsg = error.message || `Yikes! There was a problem with our hyperdrive device and we couldn't retrieve your data!`;
  console.error(errorMsg);
  return Observable.throw(errorMsg);
}
