
import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { User } from '../DTO/User';
import { ImgAccountDTO, NameAccountDTO, PasswordAccountDTO, RoleAccountDTO, EmailAccountDTO } from '../DTO/Account';





@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls:[ './account.component.css']
})

export class AccountComponent{
  user: User;
  userId: number;
  editing = false;

  nameEditing = false;
  emailEditing = false;
  passwordEditing = false;
  canChangeRole = false;
  roleEditing = false;
  imgEditing = false;

  oldPassword = "";
  newPassword = "";
  secretKey = "";
  imgLink = "";
  UserImg: string;

  emailValid = true;
  passwordValid = true;
  passwordNotSame = false;
  secretValid = true;
  thisUser = true;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.userId = +params['id'];
      
      
      this.getUserData(this.userId);
      const thisUserId = Number(localStorage.getItem('userId'));
      if (thisUserId != this.userId) {
        this.thisUser = false;
      }
      else {
        this.thisUser = true;
      }
    });

    const role = localStorage.getItem('role');
    if (role == "admin") { this.canChangeRole = true; }
    
  }

  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }


  getUserData(userId: number) {
    this.http.get<User>('https://localhost:7053/account/user?id=' + userId).subscribe(
      (response) => {
        this.user = response;
      },
      (error) => {
        console.error('Failed to fetch user data:', error);
      }
    );

  }
  editImg(): void{
    this.imgEditing = true;
  }
  editName(): void {
    this.nameEditing = true;
  }
  editEmail(): void {
    this.emailEditing = true;
  }
  editPassword(): void {
    this.passwordEditing = true;
  }
  editRole(): void {
    this.roleEditing = !this.roleEditing;
  }

  saveImg(): void {
    const body: ImgAccountDTO = {
      UserId: this.user.UserId,
      UserImg: this.imgLink
    };

    this.http.post('https://localhost:7053/account/ChangePic', body).subscribe(
      (response) => {
        this.imgEditing = false;
        this.user.UserImg = body.UserImg;
        this.imgLink = "";
      },
      (error:HttpErrorResponse) => {
        console.error('Failed to change image:', error);
      }
    );
  }
  saveName(): void {
    const body: NameAccountDTO = {
      UserId: this.user.UserId,
      UserName: this.user.UserName
    };

    this.http.post('https://localhost:7053/account/ChangeName', body).subscribe(
      (response) => {
        this.nameEditing = false;
        this.user.UserName = body.UserName;

      },
      (error: HttpErrorResponse) => {
        console.error('Failed to change name:', error);
      }
    );
  }
  saveEmail(): void {
    if (this.validateEmail(this.user.Email)) {
      this.emailValid = true;
    } else {
      this.emailValid = false;
      return;
      
    }


    const body: EmailAccountDTO = {
      UserId: this.user.UserId,
      Email: this.user.Email
    };

    this.http.post('https://localhost:7053/account/ChangeEmail', body).subscribe(
      (response) => {
        this.emailEditing = false;
        this.user.Email = body.Email;

      },
      (error: HttpErrorResponse) => {
        console.error('Failed to change email:', error);
      }
    );
    
  }
  savePassword(): void {
    if (this.oldPassword != this.user.Password) {
      this.passwordNotSame = true;
      return;
    }
    else {
      this.passwordNotSame = false;
    }
    if (this.validatePassword(this.oldPassword) && this.validatePassword(this.newPassword)) {
      this.passwordValid = true;
    } else {
      this.passwordValid = false;
      return;      
    }


    const body: PasswordAccountDTO = {
      UserId: this.user.UserId,
      PrevPassword: this.oldPassword,
      NewPassword: this.newPassword
    };

    this.http.post('https://localhost:7053/account/ChangePassword', body).subscribe(
      (response) => {
        this.passwordEditing = false;
        this.oldPassword = "";
        this.newPassword = "";
        this.user.Password = body.NewPassword;

      },
      (error: HttpErrorResponse) => {

        console.error('Failed to change email:', error);
      }
    );
  }
  saveRole(): void {
    const body: RoleAccountDTO = {
      UserId: this.user.UserId,
      Secret: this.secretKey
    };

    var link:string = "";
    if (this.user.Role == "user") {
      link = 'https://localhost:7053/account/MakeAdmin'
    }
    else {
      link = 'https://localhost:7053/account/MakeUser'
    }
    this.http.post(link, body).subscribe(
      (response) => {
        if (this.user.Role == "user") {
          this.user.Role = "admin";
        }
        else {
          this.user.Role = "user";
        }

        this.roleEditing = false;
        this.secretKey = "";
        this.secretValid = true;

      },
      (error: HttpErrorResponse) => {

        this.secretValid = false;
        console.error('Failed to change email:', error);
      }
    );
  }

  validateEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  validatePassword(password: string): boolean {
    
    const passwordRegex = /^(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/;
    return passwordRegex.test(password);
  }

  goToStat() {
    this.router.navigate(['/stat']);
  }
  goToNeeds() {
    this.router.navigate(['/needs']);
  }
}








