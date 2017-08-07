import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import {
    AlertsComponent,
    CalendarComponent,
    HeaderComponent,
    LeftComponent,
    MessagesComponent,
    TasksComponent,
    UserComponent
} from './index';

/**
 * Do not specify providers for modules that might be imported by a lazy loaded module.
 */

@NgModule({
    imports: [CommonModule, RouterModule],
    declarations: [
        AlertsComponent,
        CalendarComponent,
        HeaderComponent,
        LeftComponent,
        MessagesComponent,
        TasksComponent,
        UserComponent
    ],
    exports: [
        AlertsComponent,
        CalendarComponent,
        HeaderComponent,
        LeftComponent,
        MessagesComponent,
        TasksComponent,
        UserComponent,
        CommonModule, FormsModule, RouterModule]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [

            ]
        };
    }
}