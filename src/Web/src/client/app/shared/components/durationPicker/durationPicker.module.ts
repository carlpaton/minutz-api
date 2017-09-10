import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DurationPickerComponent } from './durationPicker.component';
@NgModule({
  imports: [
    CommonModule],
  declarations: [DurationPickerComponent],
  exports: [DurationPickerComponent]
})
export class DurationPickerModule {
}
