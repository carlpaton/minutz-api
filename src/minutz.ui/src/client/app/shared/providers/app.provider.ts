import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable} from 'rxjs/Observable';
import 'rxjs/Rx';
import { Config } from '../config/env.config';

@Injectable()
export class AppProvider {
  private _baseUrl:string = Config.API;

  constructor(private _http: Http) { }

  public exampleservice(): Observable<any> {
    let url = this._baseUrl + '';

    return this._http.get(url)
      .map((res: Response) => res.json());
  }
}
