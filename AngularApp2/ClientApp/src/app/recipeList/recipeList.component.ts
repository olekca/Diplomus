import { Recipe, SearchResult } from '../DTO/Recipe';
import { ProductShort } from '../DTO/ProductShort';
import { NeedShort } from '../DTO/NeedShort';
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
  selector: 'app-recipeList',
  templateUrl: './recipeList.component.html',
  styleUrls: ['./recipeList.component.css'],
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
export class RecipeListComponent implements OnInit {

  recipes: Recipe[];

  recipesPerPage = 10;//variables needed for pagination
  page = 1;
  maxPage: number;

  deletedRecipeId: number;//variables needed for deleting
  recipeDeleting = false;
  wrongSecret = false;
  secretKey: string;
  successDelete = false;
  secretValid = true;
  isUserAdmin = false;

  userId: number;


  @ViewChild('selectIncludedProducts') selectIncludedProducts: MatSelect;
  @ViewChild('selectExcludedProducts') selectExcludedProducts: MatSelect;
  @ViewChild('selectIncludedNeeds') selectIncludedNeeds: MatSelect;
    
  excludedProductsInput = new FormControl();
  excludedProductsSuggestions: ProductShort[]; 
  selectedExcludedProduct: ProductShort;
  excludedProducts: ProductShort[] = [];


  includedProductsInput = new FormControl();
  includedProductsSuggestions: ProductShort[];
  selectedIncludedProduct: ProductShort;
  includedProducts: ProductShort[] = [];

  includedNeedsInput = new FormControl();
  includedNeedsSuggestions: NeedShort[];
  selectedIncludedNeed: NeedShort;
  includedNeeds: NeedShort[] = [];

  searchOpen = false;

  searchLine = "";

  currentSearch: SearchRequest;
  

  noRecipes = false;

  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }


  ngOnInit(): void {
    this.getMaxPage();
    this.getPage();
    const role = localStorage.getItem('role');
    if (role == "admin") { this.isUserAdmin = true; }
    this.userId = Number(localStorage.getItem('userId'));

  }

  getPage() {
    this.http.get<Recipe[]>('https://localhost:7053/recipe/AllRecipes?page=' + this.page + '&recipesPerPage=' + this.recipesPerPage).subscribe(
      (response) => {
        this.recipes = response;
        
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);
      }
    );
  }
  getMaxPage() {
    this.http.get<number>('https://localhost:7053/recipe/recipesPageCount?page=' + this.page + '&recipesPerPage=' + this.recipesPerPage).subscribe(
      (response) => {
        this.maxPage = response;
        
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);

      }
    );
  }

  onPageChange(page: number): void {
    if (this.currentSearch != null) {
      this.sendSearchRequest(page);
      return;
    }
    this.page = page;
    this.getPage();
  }

  
  startDeleteRecipe(id: number) {
    this.deletedRecipeId = id;
    this.recipeDeleting = true;
  }
  deleteRecipe() {
    this.http.get('https://localhost:7053/recipe/DeleteRecipe?recipe_id=' + this.deletedRecipeId + '&secret=' + this.secretKey).subscribe(
      (response) => {
        this.successDelete = true;
        this.secretValid = true;
        this.secretKey = "";
        this.recipeDeleting = false;
        this.getPage();
      },
      (error: HttpErrorResponse) => {
        this.successDelete = false;
        this.secretValid = false;

      }
    );
  }


  searchIncludedProducts() {
    if (this.includedProductsInput.value) {
      this.http.get<ProductShort[]>('https://localhost:7053/search/Products', { params: { searchLine: this.includedProductsInput.value } })
        .subscribe((data) => {
          console.log(data);
          this.includedProductsSuggestions = data;
          this.selectIncludedProducts.open();
        });
    } else {
      this.includedProductsSuggestions = [];
    }
  }

  addIncludedProduct(product: ProductShort) {
    const isAlreadyIncluded = this.includedProducts.some(p => p.ProductId === product.ProductId);

    if (!isAlreadyIncluded) {
      this.includedProducts.push(product);
    }
    this.selectedIncludedProduct = null!;
    this.includedProductsInput.reset();
    this.includedProductsSuggestions = [];
  }


  searchExcludedProducts() {
    if (this.excludedProductsInput.value) {
      this.http.get<ProductShort[]>('https://localhost:7053/search/Products', { params: { searchLine: this.excludedProductsInput.value } })
        .subscribe((data) => {
          console.log(data);
          this.excludedProductsSuggestions = data;
          this.selectExcludedProducts.open();
        });
    } else {
      this.excludedProductsSuggestions = [];
    }
  }

  addExcludedProduct(product: ProductShort) {
    const isAlreadyExcluded = this.excludedProducts.some(p => p.ProductId === product.ProductId);

    if (!isAlreadyExcluded) {
      this.excludedProducts.push(product);
    }
    this.selectedExcludedProduct = null!;
    this.excludedProductsInput.reset();
    this.excludedProductsSuggestions = [];
  }



  searchNeeds() {
    if (this.includedNeedsInput.value) {
     
      this.http.get<NeedShort[]>('https://localhost:7053/search/Needs', { params: { searchLine: this.includedNeedsInput.value+"" } })
        .subscribe((data) => {
          console.log(data);
          this.includedNeedsSuggestions = data;
          this.selectIncludedNeeds.open();
        });
    } else {
      this.includedNeedsSuggestions = [];
    }
  }

  addNeed(need: NeedShort) {
    const isAlreadyIncluded = this.includedNeeds.some(n => n.NeedsId === need.NeedsId);

    if (!isAlreadyIncluded) {
      this.includedNeeds.push(need);
    }
    this.selectedIncludedNeed = null!;
    this.includedNeedsInput.reset();
    this.includedNeedsSuggestions = [];
  }

  addUsersNeeds() {
    this.http.get<NeedShort[]>('https://localhost:7053/search/userNeeds?userId=' + this.userId).subscribe(
      (response) => {
        for (let i = 0; i < response.length; ++i) {
          const isAlreadyIncluded = this.includedNeeds.some(n => n.NeedsId === response[i].NeedsId);

          if (!isAlreadyIncluded) {
            this.includedNeeds.push(response[i]);
          }
        }
      }
    );
  }


  openSearch() {
    this.searchOpen = !this.searchOpen;
  }


  sendSearchRequest(page:number) {
    const body: SearchRequest = {
      ExcludedProducts: this.excludedProducts,
      IncludedProducts: this.includedProducts,
      Needs: this.includedNeeds,
      SearchLine: this.searchLine + "",
      Page: page,
      RecipesPerPage: this.recipesPerPage
    };
    this.http.post<SearchResult>('https://localhost:7053/search/SearchByRequest', body).subscribe(
      (response) => {
        if (response.RecipesNum == 0) {
          this.noRecipes = true;
        }
        else {
          this.noRecipes = false;
        }
        this.recipes = response.Recipes;
        this.maxPage = response.MaxPage;
        this.page = response.CurPage;
        this.searchOpen = false;
      },
      (error) => {
        console.error('Failed to fetch recipes data:', error);
      }
    );
  }
  DeleteExcluded(index: number) {
    this.excludedProducts.splice(index, 1);
  }
  DeleteIncluded(index: number) {
    this.includedProducts.splice(index, 1);
  }
  DeleteNeed(index: number) {
    this.includedNeeds.splice(index, 1);
  }
  goToRecipes(recipeId: number) {
    this.router.navigate(['/recipe', recipeId, 'watch'])
  }
  goToRecipesEdit(recipeId: number) {
    this.router.navigate(['/recipe', recipeId, 'edit'])
  }
  goToRecipesAdd() {
    this.router.navigate(['/recipe', 0, 'add'])
  }
}

export interface SearchRequest {
  IncludedProducts: ProductShort[],
  ExcludedProducts: ProductShort[],
  Needs: NeedShort[],
  SearchLine: string,
  Page: number,
  RecipesPerPage:number
    }
