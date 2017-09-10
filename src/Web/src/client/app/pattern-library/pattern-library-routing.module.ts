import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PatternLibraryComponent } from './pattern-library.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: 'PatternLibrary', component: PatternLibraryComponent }
    ])
  ],
  exports: [RouterModule]
})
export class PatternLibraryRoutingModule { }
