import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    AttachmentPanelComponent
} from './attachment-panel.component';
import {
    FileuploadModule
} from '../../shared/components/fileupload/fileupload.module';
@NgModule({
    imports: [
        CommonModule,
        FileuploadModule
    ],
    declarations: [AttachmentPanelComponent],
    exports: [AttachmentPanelComponent]
})
export class AttachmentPanelModule {
}
