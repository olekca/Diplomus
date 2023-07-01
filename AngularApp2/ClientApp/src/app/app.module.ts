import { BrowserModule } from '@angular/platform-browser';

import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ExampleComponent } from './example/example.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ExpenseGuard } from './guard/expense.guard';
import { ReactiveFormsModule } from '@angular/forms'; 
import { AuthService } from './auth/auth.service';
import { RegisterComponent } from './register/register.component';
import { AccountComponent } from './account/account.component'
import { UserListComponent } from './userList/userList.component'
import { RecipeListComponent } from './recipeList/recipeList.component'
import { RecipeComponent } from './recipe/recipe.component'
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgModule} from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';

import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StatComponent } from './stat/stat.component';
import { UserNeedsComponent } from './UserNeeds/userNeeds.component'


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ExampleComponent,
    LoginComponent,
    LogoutComponent,
    RegisterComponent,
    AccountComponent,
    UserListComponent,
    RecipeListComponent,
    RecipeComponent,
    StatComponent,
    UserNeedsComponent
    
   
    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMatSelectSearchModule, MatProgressSpinnerModule,
    MatTooltipModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule,
    BrowserAnimationsModule,
    
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent, canActivate: [ExpenseGuard] },
      { path: 'logout', component: LogoutComponent, canActivate: [ExpenseGuard] },
      { path: 'register', component: RegisterComponent, canActivate: [ExpenseGuard] },
      { path: 'users', component: UserListComponent, canActivate: [ExpenseGuard] },
      { path: 'recipes', component: RecipeListComponent, canActivate: [ExpenseGuard] },
      { path: 'account/:id', component: AccountComponent, canActivate: [ExpenseGuard] },
      { path: 'recipe/:id/:mode', component: RecipeComponent, canActivate: [ExpenseGuard] },
      { path: 'stat', component: StatComponent, canActivate: [ExpenseGuard] },
      { path: 'needs', component: UserNeedsComponent, canActivate: [ExpenseGuard] },
      { path: '', component: RecipeListComponent, pathMatch: 'full', canActivate: [ExpenseGuard] },
      { path: 'counter', component: CounterComponent, canActivate: [ExpenseGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [ExpenseGuard] },
      { path: 'example', component: ExampleComponent, canActivate: [ExpenseGuard] },
    ])
  ],
  providers: [],
  exports: [
    FormsModule,
    ReactiveFormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
