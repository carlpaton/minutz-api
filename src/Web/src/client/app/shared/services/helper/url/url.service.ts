import {Injectable} from '@angular/core';

@Injectable()
export class UrlService {

  public convertQueryStringToJson(query: string): any {
    if (query && query !== '') {
      let hash: any,
        myJson: any = {},
        hashes = query.slice(query.indexOf('?') + 1).split('&');

      for (let i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        myJson[hash[0]] = hash[1];
      }
      return myJson;
    }
    return null;
  }

  public removeTokenFromUrl(): void {
    let code: string = 'code';
    let codeFound: boolean = false;
    let state: string = 'state';
    let stateFound: boolean = false;
    let url: string = window.location.href;
    let queries = url.slice(url.indexOf('?') + 1).split('&');

    for(let i = 0; i < queries.length; i++) {
      let current = queries[i].split('=');

      if(current[0] === code && !codeFound) {
        queries.splice(i, 1);
        i = -1;
        codeFound = true;
      }

      if (current[0] === state && !stateFound) {
        queries.splice(i, 1);
        i = -1;
        stateFound = true;
      }

      if (stateFound && codeFound) {
        break;
      }
    }

    if(codeFound || stateFound) {
      let newUrl = window.location.href.split('#')[0];

      if (queries.length > 0) {
        newUrl = newUrl + '?' + queries.join('&');
      }

      window.location.href = newUrl;
    }
  }
}
