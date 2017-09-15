import {
    Http,
    Request,
    RequestOptionsArgs,
    Response,
    RequestOptions,
    ConnectionBackend,
    Headers
}
    from '@angular/http';

import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Config } from '../../config/env.config';
import { LocalStorageService } from '../index';

@Injectable()
export class HttpInterceptor extends Http {

    constructor(private backend: ConnectionBackend, private defaultOptions: RequestOptions, private router: Router,
      private localStorageService: LocalStorageService) {
        super(backend, defaultOptions);
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.request(url, this.getRequestOptionArgs(options)));
    }

    get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.get(url, this.getRequestOptionArgs(options)));
    }

    post(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.post(url, body, this.getRequestOptionArgs(options)));
    }

    put(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.put(url, body, this.getRequestOptionArgs(options)));
    }

    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.delete(url, this.getRequestOptionArgs(options)));
    }

    getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {
        if (options === null) {
            options = new RequestOptions();
        }

        if (options === undefined) {
          options = new RequestOptions();
        }

        //Clearing the headers and appending the token + content type every request.
        options.headers = new Headers();

        let headers = new Headers();
        let key = 'access_token';
        let item = localStorage.getItem(key);// this.localStorageService.get(key) ? this.localStorageService.get(key) : null;
        //let parsedToken = JSON.parse(item);

        if (item) {
          headers.append('Authorization', 'Bearer ' + item);
        }
        headers.append('Content-Type', 'application/json');
        options.headers = headers;

        return options;
    }

    intercept(observable: Observable<Response>): Observable<Response> {
        return observable.catch((err, source) => {
            if (err.status === 401 && !err.endsWith(err.url, 'api/oauth/login')) {
                this.router.navigate(['/login']);
                return Observable.empty();
            } else {
                return Observable.throw(err);
            }
        });

    }
}
