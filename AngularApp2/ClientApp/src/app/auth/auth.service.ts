import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, delay, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isUserLoggedIn: boolean = false;
  userId: number = -1;
  role: string = "";

  login1(userName: string, password: string): Observable<any> {
    console.log(userName);
    console.log(password);
    this.isUserLoggedIn = userName == 'admin' && password == 'admin';
    localStorage.setItem('isUserLoggedIn', this.isUserLoggedIn ? "true" : "false");

    return of(this.isUserLoggedIn).pipe(
     // delay(1000),
      tap(val => {
        console.log("Is User Authentication is successful: " + val);
      })
    );
  }


login(email: string, password: string): Observable < boolean > {
  console.log('https://localhost:7053/account/login?email=' + email + "&password=" + password);
  return this.http.get<LoginDTO>('https://localhost:7053/account/login?email=' + email + "&password=" + password)
    .pipe(
      map(result => {
        this.isUserLoggedIn = result.IsLoggedIn;
        console.log(this.isUserLoggedIn);
        this.userId = result.UserId;
        console.log(this.userId);
        this.role = result.Role;
        console.log(this.role);
        localStorage.setItem('isUserLoggedIn', this.isUserLoggedIn ? "true" : "false");
        localStorage.setItem('userId', this.userId.toString());
        localStorage.setItem('role', this.role);
        return this.isUserLoggedIn;
      })
    );
}


  loginServer(email: string, password: string): Observable<LoginDTO> {
    return this.http.get<LoginDTO>('https://localhost:7053/account/login?email=' + email + "&password=" + password);
  }


  logout(): void {
    this.isUserLoggedIn = false;
    this.userId = -1;
    this.role = "";
    localStorage.removeItem('isUserLoggedIn');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
  }

  constructor(private http: HttpClient) { }

}
interface LoginDTO {
  IsLoggedIn: boolean,
  UserId: number,
  Role: string
}
