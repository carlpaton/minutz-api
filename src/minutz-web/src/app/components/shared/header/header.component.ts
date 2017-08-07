import { AlertsComponent } from '../alerts/alerts.component';
import { MessagesComponent } from '../messages/messages.component';
import { AuthService } from '../../../auth/auth.service';
import { UserComponent } from '../user/user.component';
import { TasksComponent } from '../tasks/tasks.component';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
   IsLoggedIn: boolean;
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
  }

}