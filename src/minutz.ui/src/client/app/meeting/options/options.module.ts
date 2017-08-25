import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OptionsComponent } from './options.component';
import { DatePickerModule } from './../../shared/components/datepicker/datepicker.module';
//import { FileSelectDirective, FileDropDirective, FileUploader} from 'ng2-file-upload';
@NgModule({
    imports: [CommonModule, FormsModule, DatePickerModule //FileUploader, FileDropDirective, FileSelectDirective
    ],
    declarations: [
        OptionsComponent
    ],
    exports: [
        OptionsComponent
    ]
})
export class OptionsModule {
}
