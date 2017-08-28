import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AppProvider } from '../shared/index';

@NgModule({
  imports: [
    CommonModule,
    HomeRoutingModule,
  //  SharedModule
  ],
  declarations: [
    HomeComponent
  ],
  exports: [
    HomeComponent
  ],
  providers: [
    AppProvider
  ]
})
export class HomeModule {
  Hello: string;
 }
