import { Environment } from '../shared/models';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EnvironmentComponent } from './environment.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: 'env', component: EnvironmentComponent }
    ])
  ],
  exports: [RouterModule]
})
export class EnvironmentRoutingModule { }
