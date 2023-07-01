import { StatDTO } from '../DTO/StatDTO';
import { Component, OnInit } from '@angular/core';
import { RecipeAmount } from '../DTO/Recipe'

import { FormGroup, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-stat',
  templateUrl: './stat.component.html',
  styleUrls:['./stat.component.css']
  
})
export class StatComponent implements OnInit {
  stats: StatDTO[];
  constructor(private authService: AuthService, private router: Router, private http: HttpClient, private route: ActivatedRoute) { }

  userId: number;
  recipes: RecipeAmount[]
  recipesEmpty = true;
  recipesNeeded = true;

  ngOnInit(): void {
    this.userId = Number(localStorage.getItem('userId'));
    if (this.userId == 0) {
     
      this.userId = 10;
    }
    this.getStatistic('https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=0');
    this.getRecipes('https://localhost:7053/recipe/statRecipes?userId=' + this.userId + '&daysAgo=0');

    
  }

  getStat() {
    this.http.get<StatDTO[]>('https://localhost:7053/dailyDiet/DummyStat').subscribe(
      (response) => {
        this.stats = response;
      },
      (error) => {
        console.error('Failed to fetch statistic data:', error);
      }
    );
  }



  getStatistic(request : string) {
    this.http.get<StatDTO[]>(request).subscribe(
      (response) => {
        this.stats = response;
      },
      (error) => {
        console.error('Failed to fetch statistic data:', error);
      }
    );
  }

  getRecipes(request: string) {
    if (request != '') {
      this.recipesNeeded = true;
      this.http.get<RecipeAmount[]>(request).subscribe(
        (response) => {
          this.recipes = response;
          console.log(this.recipes);
          if (this.recipes.length == 0) {
            this.recipesEmpty = true;
          }
          else {
            this.recipesEmpty = false;
          }
        },
        (error) => {
          console.error('Failed to recipes:', error);
        }
      );
    }
    else {
      this.recipesNeeded = false;
    }
  }

  onSelectionChange(event: Event): void {
    const selectedOption = (event.target as HTMLSelectElement).value;
    let request: string = '';
    let recipeRequest: string = '';
    switch (selectedOption) {
      //today
      case 'option1':
        request = 'https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=0';
        recipeRequest = 'https://localhost:7053/recipe/statRecipes?userId=' + this.userId + '&daysAgo=0'
        
        break;
      //Yesterday
      case 'option2':
        request = 'https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=1';
        recipeRequest = 'https://localhost:7053/recipe/statRecipes?userId=' + this.userId + '&daysAgo=1'
        break;
      //Two days ago
      case 'option3':
        request = 'https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=2';
        recipeRequest = 'https://localhost:7053/recipe/statRecipes?userId=' + this.userId + '&daysAgo=2'
        break;
      //three days ago
      case 'option4':
        request = 'https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=3';
        recipeRequest = 'https://localhost:7053/recipe/statRecipes?userId=' + this.userId + '&daysAgo=3'
        break;
      //last week
      case 'option5':
        request = 'https://localhost:7053/dailyDiet/StatPerPeriod?userId=' + this.userId + '&days=7';
        break;
      //last month
      case 'option6':
        request = 'https://localhost:7053/dailyDiet/StatPerPeriod?userId=' + this.userId + '&days=30';
        break;
      default:
        request = 'https://localhost:7053/dailyDiet/StatPerDay?userId=' + this.userId + '&daysAgo=0';
        break;
    }

    this.getRecipes(recipeRequest);
    this.getStatistic(request);
    



  }

}
