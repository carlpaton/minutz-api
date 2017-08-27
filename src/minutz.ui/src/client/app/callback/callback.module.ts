import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from './../shared/shared.module';
import { CallbackComponent } from './callback.component';
import { CallbackRoutingModule } from './callback-routing.module';

@NgModule({
  imports: [CommonModule, CallbackComponent, SharedModule],
  declarations: [CallbackComponent],
  exports: [CallbackComponent]
})
export class CallbackModule { }
