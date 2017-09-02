import { DatePickerComponent } from '../shared/components/datepicker/datepicker.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { MeetingComponent } from './meeting.component';
import { MeetingRoutingModule } from './meeting-routing.module';
import { Select2Module } from './../shared/components/select2/select2.module';
import { TimelineModule } from './../shared/components/timeline/timeline.module';
import { TimePickerModule } from './../shared/components/timepicker/timepicker.module';
import { DatePickerModule } from './../shared/components/datepicker/datepicker.module';
import { CheckboxModule } from './../shared/components/checkbox/checkbox.module';
import { CalenderModule } from './../shared/components/calender/calender.module';
import { EditorModule } from './../shared/components/editor/editor.module';
import { FileuploadModule } from './../shared/components/fileupload/fileupload.module';
import { AddButtonModule } from './../shared/components/addButton/addButton.module'; 
import { OptionsModule } from './options/options.module';
import { HeadingModule
} from './heading/heading.module';
import { AttendeesPanelModule
} from './attendeesPanel/attendees-panel.module';
import {AttendeeSelectModule
} from './../shared/components/attendeeSelect/attendeeSelect.module';
import { DurationPickerModule
} from './../shared/components/durationPicker/durationPicker.module';
import { AgendaPanelModule
} from './agendaPanel/agenda-panel.module';
import { AttachmentPanelModule
} from './attachmentPanel/attachment-panel.module';
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
    FileuploadModule,
    OptionsModule,
    HeadingModule,
    AttendeesPanelModule,
    AddButtonModule,
    TimePickerModule,
    AttendeeSelectModule,
    DurationPickerModule,
    AgendaPanelModule,
    AttachmentPanelModule
  ],
  declarations: [MeetingComponent],
  exports: [MeetingComponent]
})
export class MeetingModule {
}
