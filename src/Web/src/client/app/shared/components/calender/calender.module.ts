import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CalenderComponent } from './calender.component';


@NgModule({
    imports: [CommonModule],
    declarations: [CalenderComponent],
    exports: [CalenderComponent]
})
export class CalenderModule {
}
