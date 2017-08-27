import { Component, OnInit, AfterViewInit, ViewEncapsulation  } from '@angular/core';
import { Config } from './shared/index';
import { AuthService } from './auth/auth.service';

/**
 * This class represents the main application component. Within the @Routes annotation is the configuration of the
 * applications routes, configuring the paths for the lazy loaded components (HomeComponent, CreateComponent).
 */
//declare let $: any;
declare let AdminLTE: any;
@Component({
  moduleId: module.id,
  selector: 'sd-app',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.css']
})

export class AppComponent implements OnInit, AfterViewInit {
  bodyClasses = 'skin-blue sidebar-mini';
  body: HTMLBodyElement = document.getElementsByTagName('body')[0];
  private authenticated: boolean;
  title = 'Minutz';
  IsLoggedIn: boolean;
  Username: string = '';
  UserProfilePicture: string = '';
  constructor(public auth: AuthService) {
    if (!localStorage.getItem('access_token')) {
      auth.login();
    }
    auth.handleAuthentication();
    this.IsLoggedIn = auth.isAuthenticated();
    this.Username = localStorage.getItem('name');
    this.UserProfilePicture =  localStorage.getItem('picture');
  }

  public toggleMenu($event: any): void {
    if(this.body.classList.contains('sidebar-collapse')){
      this.body.classList.remove('sidebar-collapse')
    }else{
      this.body.classList.add('sidebar-collapse');
    }
    console.log(this.body);
  }
  ngOnInit() {
    AdminLTE.init();
  }
  public ngAfterViewInit() {}
}
