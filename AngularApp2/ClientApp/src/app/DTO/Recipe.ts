export interface RecipeDTO {
  RecipeId: number;
  RecipeName: string;
  Steps: string[];
  RecipeImg: string
  Products: RecipeProds[]
}

export interface RecipeProds {
  ProductId: number;
  RecipeProductId: number;
  ProductName: string;
  MeasureType: string;
  MeasureChar: string;
  MeasureNumber: number;

}

export interface Recipe {
  RecipeId: number;
  RecipeName: string;
  RecipeImg: string 
}

export interface RecipeAmount {
  RecipeId: number;
  RecipeName: string;
  RecipeImg: string;
  Amount: number;
}

export interface SearchResult {
  Recipes: Recipe[],
  MaxPage: number,
  CurPage: number,
  RecipesNum: number
}


