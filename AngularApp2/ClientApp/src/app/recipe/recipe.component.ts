
import { Component, OnInit, ViewChild } from '@angular/core';

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { RecipeNutrientInfo } from '../DTO/RecipeNutrientsInfo';
import { Recipe, RecipeDTO, RecipeProds } from '../DTO/Recipe';
import { ImgAccountDTO } from '../DTO/Account';
import { ProductShort } from '../DTO/ProductShort';
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
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
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

export class RecipeComponent {
  recipe: RecipeDTO;
  recipeId: number;

  nutrientInfo: RecipeNutrientInfo[];

  canChangeRecipe = false;
  editMode = false;

  stepsEdit = false;
  editedStepId:number;
  editedStep = "";

  imgEditing = false;
  imgLink = "";

  ingrEditing = false;
  ingrChoosing = false;
  editedIngr: RecipeProds = { ProductId: -1, RecipeProductId:0, ProductName: "", MeasureChar: "0", MeasureType: "", MeasureNumber: 0 };
  ingrId = -1;
  Measures: DropDownValues[];

  productsInput = new FormControl();
  productsSuggestions: ProductShort[];
  selectedProduct: ProductShort;
  @ViewChild('selectProducts') selectProducts: MatSelect;

  statAdding = false;
  statSuccess = false;
  eatenAmount: number;
  userId: number;
  mode: string;

  ngOnInit() {
    
    this.route.params.subscribe(params => {
      this.recipeId = +params['id'];
      this.mode = params['mode'];
      if (this.mode == 'edit') {
        this.getRecipeData(this.recipeId);
        this.editMode = true;
      }
      else {
        if (this.mode == 'add') {
          this.getEmptyRecipe();
          this.editMode = true;

        }
        else {
          this.getRecipeData(this.recipeId);
        }
      }
    });
   
    const role = localStorage.getItem('role');
    if (role == "admin") { this.canChangeRecipe = true; }

    this.userId = Number( localStorage.getItem('userId'));


  }

  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }

  getEmptyRecipe() {
    this.http.get<RecipeDTO>('https://localhost:7053/recipe/getEmptyRecipe').subscribe(
      (response) => {
        this.recipe = response;
      },
      (error) => {
        console.error('Failed to fetch recipe data:', error);
      }
    );
  }

  getRecipeData(recipeId: number) {
    this.http.get<RecipeDTO>('https://localhost:7053/recipe/getRecipe?recipeId=' + recipeId).subscribe(
      (response) => {
        this.recipe = response;
      },
      (error) => {
        console.error('Failed to fetch recipe data:', error);
      }
    );

  }

  editProduct(product: RecipeProds,index:number) {
    this.ingrEditing = true;
    this.editedIngr.ProductId = product.ProductId;
    this.editedIngr.ProductName = product.ProductName;
    this.editedIngr.MeasureType = product.MeasureType;
    this.editedIngr.MeasureNumber = product.MeasureNumber;
    this.editedIngr.MeasureChar = product.MeasureChar;
    this.ingrId = index;
    this.getMeasuresList(product.ProductId, product.MeasureChar);
  }

  deleteProduct(id: number) {
    this.recipe.Products.splice(id, 1);  
  }

  editRecipe() {
    this.editMode = true;
  }
  deleteStep(index: number) {
    this.recipe.Steps.splice(index, 1);
  }
  editStep(index: number) {
    if (index == -1) {
      this.editedStep = "";
    } else {
      this.editedStep = this.recipe.Steps[index];
    }
    this.stepsEdit = true;
    this.editedStepId = index;
  }

  saveStep() {
    if (this.editedStepId == -1) {
      this.recipe.Steps.push(this.editedStep);
    }
    else {
      this.recipe.Steps[this.editedStepId] = this.editedStep;
    }
    this.editedStep = "";
    this.stepsEdit = false;
  }

  editImg(): void {
    this.imgEditing = true;
  }
  saveImg(): void {
    this.imgEditing = false;
    this.recipe.RecipeImg = this.imgLink;
    this.imgLink = "";
  }
  editIngr(): void {
    this.ingrChoosing = true;
  }


  searchProducts() {
    if (this.productsInput.value) {
      this.http.get<ProductShort[]>('https://localhost:7053/search/Products', { params: { searchLine: this.productsInput.value } })
        .subscribe((data) => {
         console.log(data);
         this.productsSuggestions = data;
         this.selectProducts.open();
        });
    } else {
     this.productsSuggestions = [];
    }
  }

  getMeasuresList(id: number, mType:string) {
    this.http.get<DropDownValues[]>('https://localhost:7053/search/ProductMeasure?ProductId=' + id)
      .subscribe((data) => {
        this.Measures = data;
        this.editedIngr.MeasureChar = mType;
       
      });
  }

  addProduct(product: ProductShort) {//
    this.ingrEditing = true;
    this.ingrChoosing = false;
    this.editedIngr.ProductId = product.ProductId;
    this.editedIngr.ProductName = product.ProductName;
    this.editedIngr.MeasureNumber = 0;
    this.getMeasuresList(product.ProductId, "0");
    

  }

  addIngr() {
    if (this.ingrId == -1) {
      this.recipe.Products.push(this.editedIngr);
    }
    else {
      this.recipe.Products[this.ingrId] = this.editedIngr;
    }
    this.ingrId = -1;
    this.ingrEditing = false;
    this.editedIngr = { ProductId: -1, RecipeProductId:0, ProductName: "", MeasureChar: "0", MeasureType: "", MeasureNumber: 0 };
  }
  cancel() {
    this.ingrId = -1;
    this.ingrEditing = false;
    this.editedIngr = { ProductId: -1, RecipeProductId:0, ProductName: "", MeasureChar: "0", MeasureType: "", MeasureNumber: 0 };
  }

  sendChanges() {
    this.http.post<RecipeDTO>('https://localhost:7053/recipe/saveRecipe', this.recipe)
      .subscribe((data) => {
        
        if (this.mode == 'add') {
          
          //this.router.navigate(['/recipes'])
        }
        this.recipe = data;
        this.editMode = false;
      },
        (error) => {
        console.error('Failed to fetch recipe data:', error);
        });
    this.editMode = false;
  }

  addToStat() {
    this.statAdding = true;
    this.statSuccess = false;
  }
  cancelStat() {
    this.statAdding = false;
    this.eatenAmount = 1;
    this.statSuccess = false;
  }
  sendStat() {
    this.http.get('https://localhost:7053/dailyDiet/addStat?UserId=' + this.userId + '&RecipeId=' + this.recipeId + '&Amount=' + this.eatenAmount)
      .subscribe((data) => {

        this.statAdding = false;
        this.statSuccess = true;
        

      },
        (error) => {
          console.error('Failed to fetch recipe data:', error);
        });
  }
}

export interface DropDownValues {
  Type: string;
  Name: string;
}
