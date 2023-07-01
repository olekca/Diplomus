
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isAdmin = false;
  isUser = false;//every of this variable makes visible links for this role
  notLogged = false;
  userId: number;

  constructor(private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {

        this.setVariables();
      }
    });
    this.setVariables();
    
  }

  setVariables() {
    const role = localStorage.getItem('role');
    switch (role) {//admin has two visible parts: as user and as admin
      case "admin": this.isAdmin = true; this.isUser = true; this.notLogged = false; break;
      case "user": this.isUser = true; this.notLogged = false; break;
      default: this.notLogged = true; this.isAdmin = false; this.isUser = false; break;
    }
    this.userId = Number(localStorage.getItem('userId'));
  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
