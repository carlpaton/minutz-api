import { DatePickerComponent } from '../shared/components/datepicker/datepicker.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MeetingComponent } from './meeting.component';
import { MeetingRoutingModule } from './meeting-routing.module';
import { Select2Module } from './../shared/components/select2/select2.module';
import { TimelineModule } from './../shared/components/timeline/timeline.module';
import { DatePickerModule } from './../shared/components/datepicker/datepicker.module';
import { CheckboxModule } from './../shared/components/checkbox/checkbox.module';
import { CalenderModule } from './../shared/components/calender/calender.module';
import { EditorModule } from './../shared/components/editor/editor.module';
import { FileuploadModule } from './../shared/components/fileupload/fileupload.module';
@NgModule({
  imports: [
    CommonModule,
    MeetingRoutingModule,
    SharedModule,
    Select2Module,
    TimelineModule,
    DatePickerModule,
    CheckboxModule,
    CalenderModule,
    EditorModule,
    FileuploadModule
  ],
  declarations: [MeetingComponent],
  exports: [MeetingComponent]
})
export class MeetingModule {
}
