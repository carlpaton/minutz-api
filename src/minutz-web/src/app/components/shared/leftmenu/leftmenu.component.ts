import { AuthService } from './../../../auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-leftmenu',
  templateUrl: './leftmenu.component.html',
  styleUrls: ['./leftmenu.component.css']
})
export class LeftComponent implements OnInit {
  IsLoggedIn: boolean;
  
  constructor(public auth: AuthService) 
  {
    this.IsLoggedIn = auth.isAuthenticated();
  }

  ngOnInit() {
    
  }

}