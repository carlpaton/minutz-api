import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FileuploadComponent } from './fileupload.component';
import { FileSelectDirective, FileDropDirective, FileUploader} from 'ng2-file-upload';
@NgModule({
    imports: [CommonModule, FormsModule],
    declarations: [
        FileuploadComponent
    ],
    exports: [
        FileuploadComponent
    ]
})
export class FileuploadModule {
}
