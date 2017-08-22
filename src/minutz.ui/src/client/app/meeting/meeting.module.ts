import { DatePickerComponent } from '../shared/components/datepicker/datepicker.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MeetingComponent } from './meeting.component';
import { MeetingRoutingModule } from './meeting-routing.module';
import { Select2Module } from './../shared/components/select2/select2.module';
import { TimelineModule } from './../shared/components/timeline/timeline.module';
import { DatePickerModule } from './../shared/components/datepicker/datepicker.module';
@NgModule({
  imports: [
    CommonModule,
    MeetingRoutingModule,
    SharedModule,
    Select2Module,
    TimelineModule,
    DatePickerModule
  ],
  declarations: [MeetingComponent],
  exports: [MeetingComponent]
})
export class MeetingModule {
}
