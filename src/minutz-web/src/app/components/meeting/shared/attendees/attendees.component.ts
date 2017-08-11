import { AuthService } from './../../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-attendees',
  templateUrl: './attendees.component.html',
  styleUrls: ['./attendees.component.css']
})
export class AttendeesComponent implements OnInit {
  IsLoggedIn: boolean;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
    
  }

}