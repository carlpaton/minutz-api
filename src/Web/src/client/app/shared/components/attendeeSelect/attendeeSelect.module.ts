import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttendeeSelectComponent } from './attendeeSelect.component';
import { Select2Module } from './../select2/select2.module';
@NgModule({
    imports: [
        CommonModule,
        Select2Module],
    declarations: [AttendeeSelectComponent],
    exports: [AttendeeSelectComponent]
})
export class AttendeeSelectModule {
}
