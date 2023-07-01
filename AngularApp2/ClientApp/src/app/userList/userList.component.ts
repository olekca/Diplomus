import { User } from '../DTO/User';
import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-userList',
  templateUrl: './userList.component.html',
  styleUrls:['./userList.component.css']
  
})
export class UserListComponent implements OnInit {
  users: User[];
  usersPerPage = 20;
  page = 1;
  maxPage: number;
  deletedUserId: number;
  userDeleting = false;
  wrongSecret = false;
  secretKey: string;
  successDelete = false;
  secretValid = true;
  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }


  ngOnInit(): void {
    this.getMaxPage();
    this.getPage();
  }

  getPage() {
    this.http.get<User[]>('https://localhost:7053/account/AllUsers?page=' + this.page + '&usersPerPage=' + this.usersPerPage).subscribe(
      (response) => {
        this.users = response;
      },
      (error) => {
        console.error('Failed to fetch users data:', error);
      }
    );
  }
  getMaxPage() {
    this.http.get<number>('https://localhost:7053/account/usersPageCount?page=' + this.page + '&usersPerPage=' + this.usersPerPage).subscribe(
      (response) => {
        this.maxPage = response;
      },
      (error) => {
        console.error('Failed to fetch users data:', error);
      }
    );
  }

  onPageChange(page: number): void {
    this.page = page;
    this.getPage();
  }

  editUser(id: number) {
    this.router.navigate(['/account', id])
  }
  /*deletedUserId: number;
  userDeleting = false;
  wrongSecret = false;
  secretKey: string;
  successDelete = false;*/
  startDeleteUser(id: number) {
    this.deletedUserId = id;
    this.userDeleting = true;
  }
  deleteUser() {
    this.http.get('https://localhost:7053/account/DeleteUser?user_id=' + this.deletedUserId + '&secret=' + this.secretKey).subscribe(
      (response) => {
        this.successDelete = true;
        this.secretValid = true;
        this.secretKey = "";
        this.userDeleting = false;
        this.getPage();
      },
      (error: HttpErrorResponse) => {
        this.successDelete = false;
        this.secretValid = false;
        
      }
    );
  }
}
