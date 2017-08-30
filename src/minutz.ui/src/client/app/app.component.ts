import {
  Component,
  OnInit,
  AfterViewInit,
  ViewEncapsulation
} from '@angular/core';
import { Config } from './shared/index';
import { AuthService } from './auth/auth.service';
//declare let $: any;
declare let AdminLTE: any;
@Component({
  moduleId: module.id,
  selector: 'sd-app',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  public bodyClasses: string = 'skin-blue sidebar-mini';
  public body: HTMLBodyElement = document.getElementsByTagName('body')[0];
  public title: string = 'Minutz';
  public IsLoggedIn: boolean;
  public Username: string = '';
  public UserProfilePicture: string = '';
  public toggleMenu($event: any): void {
    if (this.body.classList.contains('sidebar-collapse')) {
      this.body.classList.remove('sidebar-collapse')
    } else {
      this.body.classList.add('sidebar-collapse');
    }
    console.log(this.body);
  }
  public ngOnInit() {
    AdminLTE.init();
  }
  public ngAfterViewInit() { }
  private authenticated: boolean;
  private constructor(public auth: AuthService) {
    if (!localStorage.getItem('access_token')) {
      auth.login();
    }
    auth.handleAuthentication();
    this.IsLoggedIn = auth.isAuthenticated();
    this.Username = localStorage.getItem('name');
    this.UserProfilePicture = localStorage.getItem('picture');
  }
}
