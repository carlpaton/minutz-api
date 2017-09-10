import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { PatternLibraryComponent } from './pattern-library.component';
import { PatternLibraryRoutingModule } from './pattern-library-routing.module';

@NgModule({
  imports: [CommonModule, PatternLibraryRoutingModule, SharedModule],
  declarations: [PatternLibraryComponent],
  exports: [PatternLibraryComponent]
})
export class PatternLibraryModule { }
