import { AuthHttpRequestService } from './../services/extensions/app.http.wrapper';
import { Http, Response,  RequestMethod } from '@angular/http';
import { Environment } from '../models/index';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/Rx';

@Injectable()
export class EnvironmentsRepository {
    constructor(private _http: AuthHttpRequestService) {
    }
    getAll(destinationServerApikey: string, destinationServerUrl: string): Observable<Environment[]> {
    let environments$ = this._http
      .processRequest(`api/environments?serverUrl=${destinationServerUrl}&serverApikey=${destinationServerApikey}`,
        RequestMethod.Get,null,null)
      .map((environmentData: any) => {
        let response: Array<Environment> = [];
        console.log(JSON.parse(environmentData._body));
        if (environmentData) {
          JSON.parse(environmentData._body).forEach((r: any) => {
            var d = new Environment();
            d.Description = r.Description;
            d.DestinationServerApikey = r.DestinationServerApikey;
            d.DestinationServerUrl = r.DestinationServerUrl;
            d.Id = r.Id;
            d.Name = r.Name;
            d.SortOrder = r.SortOrder;
            d.UseGuidedFailure = r.UseGuidedFailure;
            response.push(d);
          });
        }
        return response;
      })
      .catch(handleError);
      return environments$;
    }
}
function handleError (error: any) {
  let errorMsg = error.message || `Yikes! There was a problem with our hyperdrive device and we couldn't retrieve your data!`;
  console.error(errorMsg);
  return Observable.throw(errorMsg);
}
