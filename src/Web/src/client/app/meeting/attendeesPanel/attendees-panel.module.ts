import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
    AttendeesPanelComponent
} from './attendees-panel.component';
import {
    AttendeeSelectModule
} from '../../shared/components/attendeeSelect/attendeeSelect.module';
import {
    AddButtonModule
} from '../../shared/components/addButton/addButton.module';
import {MeetingRepository} from '../../shared/repositories/meeting.repository';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        AddButtonModule,
        AttendeeSelectModule
        //FileUploader, FileDropDirective, FileSelectDirective
    ],
    providers: [
        MeetingRepository
    ],
    declarations: [
        AttendeesPanelComponent
    ],
    exports: [
        AttendeesPanelComponent
    ]
})
export class AttendeesPanelModule {
}
