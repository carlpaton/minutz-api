import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgendaPanelComponent } from './agenda-panel.component';
import {
    AddButtonModule
} from '../../shared/components/addButton/addButton.module';
@NgModule({
    imports: [CommonModule, AddButtonModule],
    declarations: [AgendaPanelComponent],
    exports: [AgendaPanelComponent]
})
export class AgendaPanelModule {
}
