import { Injectable } from '@angular/core';
import { Http, Response,  RequestMethod } from '@angular/http';
import { AuthHttpRequestService } from './../services/extensions/app.http.wrapper';
import { Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/Rx';
import { Integration } from '../models/index';
@Injectable()
export class IntegrationsRepository {
  constructor(private _http: AuthHttpRequestService) { 
  }
  getAll(): Observable<Integration[]> {
    let roles$ = this._http
      .processRequest(`api/integrations`,RequestMethod.Get, null, null)
      .map((integrationData: any) => {
        let response: Array<Integration> = [];
        if (integrationData) {
          JSON.parse(integrationData._body).forEach((r: any) => {
            response.push(new Integration(r.Key, r.Value));
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
