
import { NeedShort } from '../DTO/NeedShort';
import { NeedDTO } from '../DTO/NeedDTO';
import { Component, OnInit, ViewChild } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgModule } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';

import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-userNeeds',
  templateUrl: './userNeeds.component.html',
  styleUrls: ['./userNeeds.component.css'],
  animations: [
    trigger('transitionMessages', [
      // Animation configuration for @transitionMessages
      // ...
    ]),
    trigger('transformPanel', [
      // Animation configuration for @transformPanel
      // ...
    ])
  ]


})
export class UserNeedsComponent implements OnInit {

  needs: NeedDTO[] = [];
  clickedNeed: NeedDTO = { NeedsId: 0, NeedsName: "", Desc:"" };
  userId: number;





  @ViewChild('selectNeeds') selectNeeds: MatSelect;
    
  /*
     @ViewChild('selectIncludedProducts') selectIncludedProducts: MatSelect;
  @ViewChild('selectExcludedProducts') selectExcludedProducts: MatSelect;

   excludedProductsInput = new FormControl();
  excludedProductsSuggestions: ProductShort[]; 
  selectedExcludedProduct: ProductShort;
  excludedProducts: ProductShort[] = [];


  includedProductsInput = new FormControl();
  includedProductsSuggestions: ProductShort[];
  selectedIncludedProduct: ProductShort;
  includedProducts: ProductShort[] = [];*/

  needsInput = new FormControl();
  needsSuggestions: NeedShort[];
  selectedNeed: NeedShort;

  includedNeeds: NeedShort[] = [];

  addingMode = false;
  editingMode = false;

  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }


  ngOnInit(): void {
    
    this.userId = Number(localStorage.getItem('userId'));
    if (this.userId == 0) { this.userId = 10 }
    this.getNeeds();
  }

  getNeeds() {
    this.http.get<NeedDTO[]>('https://localhost:7053/account/userNeeds?userId=' + this.userId).subscribe(
      (response) => {
        this.needs = response;
        
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);
      }
    );
  }
  

  
  /*deletedUserId: number;
  userDeleting = false;
  wrongSecret = false;
  secretKey: string;
  successDelete = false;*/



 


  clickNeed(id: number) {
    this.clickedNeed = this.needs[id];
  }


  searchNeeds() {
    if (this.needsInput.value) {

      this.http.get<NeedShort[]>('https://localhost:7053/search/Needs', { params: { searchLine: this.needsInput.value+"" } })
        .subscribe((data) => {
          console.log(data);
          this.needsSuggestions = data;
          this.selectNeeds.open();
        });
    } else {
      this.needsSuggestions = [];
    }
  }

  addNeed(need: NeedShort) {
    const isAlreadyIncluded = this.needs.some(n => n.NeedsId === need.NeedsId);
    if (!isAlreadyIncluded) {
      let newNeed: NeedDTO;
      this.http.get<NeedDTO>('https://localhost:7053/search/NeedDesc?needId=' + need.NeedsId).subscribe(
        (response) => {
          this.needs.push(response);
        });
    }
    this.selectedNeed = null!;
    this.needsInput.reset();
    this.needsSuggestions = [];
  }

  /*addNeed(need: NeedShort) {
    
    alert("finally " + this.needs.length + this.includedNeeds.length);
    const isAlreadyIncluded = this.includedNeeds.some(n => n.NeedsId === need.NeedsId);
    alert(isAlreadyIncluded);
    if (!isAlreadyIncluded) {
      this.includedNeeds.push(need);
    }
    this.selectedNeed = null!;
    this.needsInput.reset();
    this.needsSuggestions = [];
  }*/
  /*
   export interface NeedDTO {
  NeedsId: number,
  NeedsName: string,
  Desc:string
}*/

 /* sendSearchRequest() {
    alert("try to send");
    const body: SearchRequest = {
      ExcludedProducts: this.excludedProducts,
      IncludedProducts: this.includedProducts,
      Needs: this.includedNeeds,
      SearchLine: this.searchLine + ""
    };
    this.http.post<Recipe[]>('https://localhost:7053/search/SearchByRequest', body).subscribe(
      (response) => {
        this.recipes = response;
        this.searchOpen = false;
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);
      }
    );
  }*/

  sendRequest() {
    const body: UserNeedsUpdateDTO = {
      UserId: this.userId,
      Needs: this.needs

    };
    this.http.post<NeedDTO[]>('https://localhost:7053/account/updateNeeds', body).subscribe(
      (response) => {
        this.needs = response;
        this.addingMode = false;
        this.editingMode = false;
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);
      }
    );
  }
  DeleteExcluded(id: number) {
   // this.excludedProducts = this.excludedProducts.filter(prod => prod.ProductId !== id);
  }
  DeleteNeed(id: number) {
    this.needs.splice(id, 1);
  }

  setAddingMode() {
    this.addingMode = true;
  }
  setEditingMode() {
    this.editingMode = true;
  }

  goToAccount() {
    this.router.navigate(['/account/' + this.userId]);
  }
  goToStat() {
    this.router.navigate(['/stat']);
  }
}
export interface UserNeedsUpdateDTO {
  UserId: number;
  Needs: NeedDTO[];
}

