import { Injectable } from '@angular/core';
import { Http, Response,  RequestMethod } from '@angular/http';
import { AuthHttpRequestService } from './../services/extensions/app.http.wrapper';
import { Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/Rx';
import { MachinePolicy } from './../models/index';
@Injectable()
export class MachinePolicyRepository {
  constructor(private _http: AuthHttpRequestService) {
  }
  getAll(): Observable<MachinePolicy[]> {
    let roles$ = this._http
      .processRequest(`api/machinepolicies`,RequestMethod.Get, null, null)
      .map((integrationData: any) => {
        let response: Array<MachinePolicy> = [];
        if (integrationData) {
          JSON.parse(integrationData._body).forEach((r: any) => {
            var current = new MachinePolicy();
            current.Id = r.Id;
            current.Name = r.Name;
            response.push(current);
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
