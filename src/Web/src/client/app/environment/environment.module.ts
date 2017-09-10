import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { EnvironmentComponent } from './environment.component';
import { EnvironmentRoutingModule } from './environment-routing.module';

@NgModule({
  imports: [CommonModule, EnvironmentRoutingModule, SharedModule],
  declarations: [EnvironmentComponent],
  exports: [EnvironmentComponent]
})
export class EnvironmentModule { }
