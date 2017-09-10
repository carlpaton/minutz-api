import { Injectable } from '@angular/core';
import { Http, Response, RequestMethod } from '@angular/http';
import { AuthHttpRequestService } from './../services/extensions/app.http.wrapper';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/Rx';
import { Role, Machine, MachinePolicy } from '../models/index';

@Injectable()
export class MachineRepository {

  constructor(private _http: AuthHttpRequestService) {

  }

  getAll(): Observable<Role[]> {
    let roles$ = this._http.processRequest(`api/roles`, RequestMethod.Get,null, null)
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

  getMachinePolocies(): Observable<MachinePolicy[]> {
    let policies$ = this._http.processRequest(`api/machinepolicies`, RequestMethod.Get, null, null)
      .map((policiesData: any) => {
        let response: Array<MachinePolicy> = [];

        if (policiesData) {
          JSON.parse(policiesData._body).forEach((p: MachinePolicy) => {
            var data = new MachinePolicy();
            data.Name = p.Name;
            data.Id = p.Id;
            response.push(data);
          });
        }
        return response;
      })
      .catch(handleError);
    return policies$;
  }

  saveMachine(machine: Machine): Observable<Machine> {
    let machine$ = this._http.processRequest(`api/machine`, RequestMethod.Put, null, machine)
      .map((machineData: any) => {
        let response: Machine;
        if (machineData) {
          console.log(machineData);
          response.DestinationServerApikey = machineData._body.apiKey;
        }
      }).catch(handleError);
    return machine$;
  }
}

function handleError(error: any) {
  let errorMsg = error.message || `Yikes! There was a problem with our hyperdrive device and we couldn't retrieve your data!`;
  console.error(errorMsg);
  return Observable.throw(errorMsg);
}
