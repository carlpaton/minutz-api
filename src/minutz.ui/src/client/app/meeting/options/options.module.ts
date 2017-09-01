import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OptionsComponent } from './options.component';
import { DatePickerModule } from './../../shared/components/datepicker/datepicker.module';
import { TimePickerModule } from './../../shared/components/timepicker/timepicker.module';
//import { FileSelectDirective, FileDropDirective, FileUploader} from 'ng2-file-upload';
import { ToggleModule } from './../../shared/components/toggle/toggle.module';
import { DurationPickerModule } from './../../shared/components/durationPicker/durationPicker.module';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        TimePickerModule,
        DatePickerModule, ToggleModule, DurationPickerModule //FileUploader, FileDropDirective, FileSelectDirective
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
