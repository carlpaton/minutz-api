import { AlertsComponent } from './components/shared/alerts/alerts.component';
import { CreateMeetingComponent } from './components/meeting/create/createmeeting.component';
import { DiaryComponent } from './components/diary/diary.component';
import { MeetingComponent } from './components/meeting/meeting.component';
import { EditMeetingComponent } from './components/meeting/edit/editmeeting.component';
import { RunMeetingComponent } from './components/meeting/run/runmeeting.component';
import { UserProfileComponent } from './components/userprofile/userprofile.component';
import { AdminComponent } from './components/admin/admin.component';
import { HomeTasksComponent } from './home/hometask/hometasks.component';
import { HomeMeetingsComponent } from './home/homemeetings/homemeetings.component';
import { CallbackComponent } from './callback/callback.component';
import { BrowserModule } from '@angular/platform-browser';
import { HomeComponent } from './home/home.component';
import { AuthService } from './auth/auth.service';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { ROUTES } from './app.routes';
import { SharedModule } from './components/shared/shared.module';

import { routing } from './app.routes';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CallbackComponent,
    HomeTasksComponent,
    HomeMeetingsComponent,
    MeetingComponent,
    RunMeetingComponent,
    EditMeetingComponent,
    CreateMeetingComponent,
    DiaryComponent,
    AdminComponent,
    UserProfileComponent
    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    SharedModule.forRoot(),
    RouterModule.forRoot(ROUTES)
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
