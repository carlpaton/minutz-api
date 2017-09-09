import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DemoComponent } from './demo.component';
import { DemoRoutingModule } from './demo-routing.module';

@NgModule({
  imports: [CommonModule, DemoRoutingModule, SharedModule],
  declarations: [DemoComponent],
  exports: [DemoComponent]
})
export class DemoModule { }
