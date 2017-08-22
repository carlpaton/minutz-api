import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatePickerComponent } from './datepicker.component';


@NgModule({
    imports: [CommonModule],
    declarations: [DatePickerComponent],
    exports: [DatePickerComponent]
})
export class DatePickerModule {
}
