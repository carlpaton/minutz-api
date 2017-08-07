import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CallbackComponent } from './callback/callback.component';
import { HomeComponent } from './home/home.component';
import { MeetingComponent } from '../app/components/meeting/meeting.component'
import { DiaryComponent } from './components/diary/diary.component';
import { AdminComponent } from './components/admin/admin.component';
import { UserProfileComponent } from './components/userprofile/userprofile.component';

export const ROUTES: Routes = [
  { 
    path: '', component: HomeComponent 
  },
  { path: 'callback', component: CallbackComponent },
  { path : 'meeting', component : MeetingComponent},
  { path : 'diary', component : DiaryComponent},
  { path : 'profile', component : UserProfileComponent},
  { path : 'admin', component : AdminComponent},
  { path: '**', redirectTo: '' }
];
export const routing: ModuleWithProviders = RouterModule.forRoot(ROUTES);