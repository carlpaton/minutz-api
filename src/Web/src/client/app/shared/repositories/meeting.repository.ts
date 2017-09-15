import {
    Injectable
} from '@angular/core';
import {
    Observable
} from 'rxjs/Observable';
import {
    Http,
    Response,
    RequestMethod,
    URLSearchParams
} from '@angular/http';
import {AuthHttpRequestService} from '../services/extensions/app.http.wrapper';
import {
    MeetingModel
} from '../models/meetingModel';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/Rx';

@Injectable()
export class MeetingRepository {
    constructor(private _http: AuthHttpRequestService){

    }
    getAllMeetings(): Observable<MeetingModel[]> {
        let params = new URLSearchParams();
        
        let meetings$ = this._http.processRequest(`/meeting`,RequestMethod.Get,params,null)
        .map((meetingsData: any)=> {

        }).catch(handleError);
        return meetings$;
    }
}
function handleError(error: any) {
    let errorMsg = error.message || `Yikes! There was a problem with our hyperdrive device and we couldn't retrieve your data!`;
    console.error(errorMsg);
    return Observable.throw(errorMsg);
}