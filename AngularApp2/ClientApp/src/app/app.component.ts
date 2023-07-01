import { Component } from '@angular/core';
import { AuthService } from './auth/auth.service';
import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls:['./app.component.css']
})
export class AppComponent {
  title = 'app';
  isUserLoggedIn = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    let storeData = localStorage.getItem("isUserLoggedIn");
    console.log("StoreData: " + storeData);

    if (storeData != null && storeData == "true")
      this.isUserLoggedIn = true;
    else


      this.isUserLoggedIn = false;
  }
}
