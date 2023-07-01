import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule, Validators, NgControl, NgForm, NgModel, Validator, AbstractControl, ValidatorFn } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import { User } from '../DTO/User'








@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls:['./register.component.css'],
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `],


})
export class RegisterComponent implements OnInit {

  userName: string;
  password: string;
  confirmedPassword: string;
  userEmail: string;
  registerForm: FormGroup;
  difPasswords = false;
  userExists = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      "userName": new FormControl('', Validators.required),
      "password": new FormControl('', [Validators.required, Validators.pattern('(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$')]),
      "userEmail": new FormControl('', [Validators.required, Validators.email]),
      "confirmedPassword": new FormControl('', [Validators.required, Validators.pattern('(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$')]),
    });
  }

  submit() {

    if (this.registerForm.get('confirmedPassword')!.value != this.registerForm.get('password')!.value) {
      this.difPasswords = true;
      return;
    } else {
      this.difPasswords = false;
    }
    const myUser = {
      UserName: this.registerForm.get('userName')!.value,
      Email: this.registerForm.get('userEmail')!.value,
      Password: this.registerForm.get('password')!.value
    };


    

    this.authService.register(myUser)
      .subscribe(data => {
        if (data==true) {
          this.router.navigate(['/counter']);
        }
        else {
          this.userExists = true;
        }
      });

    console.log(this.registerForm.get('userName')!.value);
    console.log(this.registerForm.get('userEmail')!.value);
    console.log(this.registerForm.get('password')!.value);
    console.log(this.registerForm.get('confirmedPassword')!.value);
  }
  onClickSubmit(data: any) {
    this.userName = data.userName;
    this.password = data.password;

    console.log("Login page: " + this.userName);
    console.log("Login page: " + this.password);

    this.authService.login(this.userName, this.password)
      .subscribe(data => {
        if (data) this.router.navigate(['/recipes']);
      });
  }

  goToLogIn() {
    this.router.navigate(['/login']);
  }

}




