import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';




@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
  

})
export class LoginComponent implements OnInit {

  userName: string;
  password: string;
  formData: FormGroup;
  loginFail = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.formData = new FormGroup({
      userName: new FormControl(""),
      password: new FormControl(""),
    });
  }

  onClickSubmit(data: any) {
    this.userName = data.userName;
    this.password = data.password;

    this.authService.login(this.userName, this.password)
      .subscribe(data => {
        if (data) {
          this.loginFail = false;
          this.router.navigate(['/recipes']);

        }
        else {
          this.loginFail = true;
        }
      });
  }
  goToSignUp() {
    this.router.navigate(['/register']);
  }
}
