import {Injectable} from '@angular/core';
import {Http, Headers, URLSearchParams, Request, RequestMethod, Response} from '@angular/http';
import {Config} from '../../config/env.config';

@Injectable()
export class AuthHttpRequestService {
    constructor(private http: Http) { }

    public processRequest(url: string, method: RequestMethod, searchParams: URLSearchParams, body: any) {
        let headers = new Headers();
        let key = 'auth.jwt.token';
        let item = localStorage.getItem(key) ? localStorage.getItem(key) : null;
        let userToken = JSON.parse(item);

        if (body !== null) {
            if (body.model && body.files) {
                headers.append('Content-Type', undefined);
            } else {
                headers.append('Content-Type', 'application/json');
            }
        }

        if (userToken && userToken.token) {
            headers.append('Authorization', 'Bearer ' + userToken.token);
        }

        return this.http.request(new Request({
            method: method,
            headers: headers,
            body: this.transformRequest(body, method),
            url: Config.API + url,
            search: searchParams,
        }));
    }

    private transformRequest(body: any, method: RequestMethod) {
        if (method === RequestMethod.Post) {
            if (body.files && body.model) {
                let formData = new FormData();
                formData.append('model', JSON.stringify(body.model));
                for (let i = 0; i < body.files.length; i++) {
                    formData.append('file' + i, body.files[i]);
                }
                return formData;
            } else {
                return JSON.stringify(body);
            }
        }

        return body;
    }
}
