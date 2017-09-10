import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AttendeesPanelComponent } from './attendees-panel.component';
//import { FileSelectDirective, FileDropDirective, FileUploader} from 'ng2-file-upload';
@NgModule({
    imports: [CommonModule, FormsModule, //FileUploader, FileDropDirective, FileSelectDirective
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
