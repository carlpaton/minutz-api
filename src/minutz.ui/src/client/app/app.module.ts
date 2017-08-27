import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { APP_BASE_HREF } from '@angular/common';
import { XHRBackend, RequestOptions, Http, HttpModule } from '@angular/http';
import { Router } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRoutingModule} from './app-routing.module';
import { AuthService } from './auth/auth.service';
import { CallbackModule } from './callback/callback.module';
import { Select2Module } from './shared/components/select2/select2.module';
import { EnvironmentModule} from './environment/environment.module';
import {
  IntegrationsRepository,
  EnvironmentsRepository,
  MachinePolicyRepository,
  MachineRepository,
  RoleRepository } from './shared/repositories/index';
import {
  Environment,
  Integration,
  Machine,
  MachinePolicy,
  Select2Model,
  Role } from './shared/models/index';
import {
  LocalStorageService,
  AuthenticationService,
  AuthenticationTokenService,
  AuthHttpRequestService,
  BaseHttpComponent,
  HttpInterceptor,
  RoleService,
  UrlService } from './shared/services/index';
import { AboutModule } from './about/about.module';
import { PatternLibraryModule } from './pattern-library/pattern-library.module';
import { HomeModule } from './home/home.module';
import { DemoModule } from './demo/demo.module';
import { MeetingModule } from './meeting/meeting.module';
import { NotFoundModule } from './shared/components/notfound/notfound.module';
import { UnauthorizedModule } from './shared/components/unauthorized/unauthorized.module';
import { SharedModule } from './shared/shared.module';
@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    AboutModule,
    PatternLibraryModule,
    HomeModule,
    DemoModule,
    EnvironmentModule,
    Select2Module,
    MeetingModule,
    //CallbackModule,
  //UnauthorizedModule, 
  NotFoundModule, HttpModule, SharedModule.forRoot()],
  declarations: [AppComponent],
  providers: [{
    provide: APP_BASE_HREF,
    useValue: '<%= APP_BASE %>'
  },
    IntegrationsRepository,
    EnvironmentsRepository,
    MachinePolicyRepository,
    MachineRepository,
    RoleRepository,
    //AuthenticationService,
    LocalStorageService,
    //AuthenticationTokenService,
    RoleService,
    UrlService,
    AuthService,
    AuthHttpRequestService,
    {
      provide: Http, useFactory: (xhrBackend: XHRBackend, requestOptions: RequestOptions,
        router: Router, localStorageService: LocalStorageService) =>
      new HttpInterceptor(xhrBackend, requestOptions, router, localStorageService),
      deps: [XHRBackend, RequestOptions, Router]
    },
    {
      provide: BaseHttpComponent, useFactory: (xhrBackend: XHRBackend, requestOptions: RequestOptions) =>
      new Http(xhrBackend, requestOptions),
      deps: [XHRBackend, RequestOptions, Router]
    },
    {
      provide: HttpInterceptor, useFactory: (xhrBackend: XHRBackend, requestOptions: RequestOptions,
        router: Router, localStorageService: LocalStorageService) =>
      new HttpInterceptor(xhrBackend, requestOptions, router, localStorageService),
      deps: [XHRBackend, RequestOptions, Router]
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
