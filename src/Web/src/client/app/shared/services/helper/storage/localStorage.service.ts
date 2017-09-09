import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {
  public expired(date: string, expiry: number): boolean {
    return (+new Date() - +new Date(date)) / 1000 / 60 / 60 >= expiry;
  }

  public clearAll() {
    localStorage.clear();
  }

  public clear(key: string) {
    if (key && key !== '') {
      if (localStorage.getItem(key)) {
        localStorage.removeItem(key);
      }
    }
  }

  public getRaw(key: string): any {
    if (key && key !== '') {
      let item = localStorage.getItem(key);
      // this looks strange, but it is correct
      if (item && item !== 'null') {
        return item;
      }
    }
    return null;
  }

  public get(key: string) {
    if (key && key !== '') {
      let item = localStorage.getItem(key) ? localStorage.getItem(key) : null;
      if (item && item !== 'null') {
        return JSON.parse(item);
      }
    }
    return null;
  }

  public set(key: string, value: any) {
    if (key && key !== '') {
      if (value === undefined) {
        value = null;
      }

      localStorage.setItem(key, JSON.stringify(value));
    }
  }
}
