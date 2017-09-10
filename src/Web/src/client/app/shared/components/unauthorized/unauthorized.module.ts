import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { UnauthorizedComponent } from './unauthorized.component';
import { UnauthorizedRoutingModule } from './unauthorized-routing.module';

@NgModule({
  imports: [CommonModule, UnauthorizedRoutingModule, SharedModule],
  declarations: [UnauthorizedComponent],
  exports: [UnauthorizedComponent]
})
export class UnauthorizedModule { }
