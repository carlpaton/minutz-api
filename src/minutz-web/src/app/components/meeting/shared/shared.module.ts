import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import {
    MeetingHeadingComponent,
    MeetingOptionsComponent,
    AttendeesComponent,
    AgendaComponent,
    AttachmentsComponent,
    MeetingTabZoneComponent
} from './index';

@NgModule({
    imports: [CommonModule, RouterModule],
    declarations: [
        MeetingHeadingComponent,
        MeetingOptionsComponent,
        AttendeesComponent,
        AttachmentsComponent,
        MeetingTabZoneComponent,
        AgendaComponent
    ],
    exports: [
        MeetingHeadingComponent,
        MeetingOptionsComponent,
        AttendeesComponent,
        AgendaComponent,
        AttachmentsComponent,
        MeetingTabZoneComponent,
        CommonModule, FormsModule, RouterModule]
})
export class SharedMeetingModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedMeetingModule,
            providers: [

            ]
        };
    }
}