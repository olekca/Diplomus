import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ExpenseGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree {
    let url: string = state.url;

    return this.checkLogin(url);
  }

  checkLogin(url: string): true | UrlTree {
    console.log("Url: " + url)
    let isLogged: string = localStorage.getItem('isUserLoggedIn')!;
    let userId: number = Number.parseInt(localStorage.getItem('userId')!);
    let role: string = localStorage.getItem('role')!;

    if (isLogged != null && isLogged == "true") {
      if (url == "/login") {
        if (role == "admin") {
          this.router.parseUrl('/example');
        }
        else {
          this.router.parseUrl('');
        }
        
      }      
      else
        return true;
    } else {
      return this.router.parseUrl('/login');
    }
    return this.router.parseUrl('/login');
  }
}
