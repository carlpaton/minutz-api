import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimeZoneComponent } from './timezone.component';
@NgModule({
  imports: [
    CommonModule],
  declarations: [TimeZoneComponent],
  exports: [TimeZoneComponent]
})
export class TimeZoneModule {
}
