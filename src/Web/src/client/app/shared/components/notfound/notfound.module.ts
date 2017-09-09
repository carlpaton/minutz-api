import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { NotFoundComponent } from './notfound.component';
import { NotFoundRoutingModule } from './notfound-routing.module';

@NgModule({
  imports: [CommonModule, NotFoundRoutingModule, SharedModule],
  declarations: [NotFoundComponent],
  exports: [NotFoundComponent]
})
export class NotFoundModule { }
