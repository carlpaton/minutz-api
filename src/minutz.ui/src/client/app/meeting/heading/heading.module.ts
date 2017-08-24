import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeadingComponent } from './heading.component';
//import { FileSelectDirective, FileDropDirective, FileUploader} from 'ng2-file-upload';
@NgModule({
    imports: [CommonModule, FormsModule, //FileUploader, FileDropDirective, FileSelectDirective
    ],
    declarations: [
        HeadingComponent
    ],
    exports: [
        HeadingComponent
    ]
})
export class HeadingModule {
}
