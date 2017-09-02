import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgendaPanelComponent } from './agenda-panel.component';
@NgModule({
    imports: [CommonModule],
    declarations: [AgendaPanelComponent],
    exports: [AgendaPanelComponent]
})
export class AgendaPanelModule {
}
