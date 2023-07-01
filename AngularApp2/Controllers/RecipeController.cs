using AngularApp2.Models.Entity;
using AngularApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;



namespace AngularApp2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public RecipeController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }


        [HttpGet("page")]
        public string getRecipes(int lastIndex)//not used
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                List<Recipes> nextPage = db.Recipes
                .OrderBy(r => r.RecipeId)
                .Where(b => b.RecipeId > lastIndex)
                .Take(10)
                .ToList();
                return JsonConvert.SerializeObject(nextPage);
            }
        }

        [HttpGet("AllRecipes")]

        public string getRecipes(int page, int recipesPerPage)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                List<Recipes> nextPage = db.Recipes
                .OrderBy(r => r.RecipeId)
                .Skip((page - 1) * recipesPerPage)
                .Take(recipesPerPage)
                .ToList();
                List<RecipeDTO> res = _mapper.Map<List<RecipeDTO>>(nextPage);
                return JsonConvert.SerializeObject(res);
            }
        }

        [HttpGet("recipesPageCount")]
        public int userPagesCount(int recipesPerPage)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                return (int)Math.Ceiling(db.Recipes.Count() / (double)recipesPerPage);
            }
        }

        [HttpGet("getRecipe")]
        public string getRecipe(int recipeId)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
              
                Recipes recipe = db.Recipes
                    .Include(r => r.RecipesProducts)
                    .ThenInclude(p=> p.Product)
                    .FirstOrDefault(r => r.RecipeId == recipeId);
                RecipeProds[] i = (from prod in recipe.RecipesProducts
                                   select new RecipeProds
                                   {
                                       MeasureNumber = prod.MeasureNumber,
                                       MeasureType = prod.Product.getMeasureString(prod.MeasureType),
                                       MeasureChar = prod.MeasureType,
                                       ProductId = prod.ProductId,
                                       ProductName = prod.Product.ProductName,
                                       RecipeProductId = prod.RecipeProductId
                                   }).ToArray();

                RecipeDTO res = _mapper.Map<RecipeDTO>(recipe);
                res.Products = i;
                return JsonConvert.SerializeObject(res);
            }
            
        }

        [HttpGet("DeleteRecipe")]
        public IActionResult DeleteRecipe(int recipe_id, string secret)
        {
            if (secret != _configuration.GetValue<String>("secret"))
            {
                return StatusCode(403);
            }
            using (DiplomusContext db = new DiplomusContext())
            {
                Recipes recipe = db.Recipes
                    .Where(r => r.RecipeId == recipe_id)
                    .FirstOrDefault();
                if (recipe != null)
                {
                    db.Recipes.Remove(recipe);
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }

        [HttpGet("getEmptyRecipe")]
        public string getEmptyRecipe()
        {
            RecipeDTO recipe = new RecipeDTO();
            recipe.RecipeId = 0;
            recipe.RecipeName = "New recipe";
            recipe.Steps = new string[] {};
            recipe.RecipeImg = "https://fomantic-ui.com/images/wireframe/image.png";
            recipe.Products = new RecipeProds[] {};
            return JsonConvert.SerializeObject(recipe);
        }

        [HttpPost("SaveRecipe")]
        public string SaveRecipe(RecipeDTO recipe)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Recipes newRecipe;
                if (recipe.RecipeId == 0)
                {
                    newRecipe = new Recipes();
                    db.Recipes.Add(newRecipe);
                }
                else
                {
                    newRecipe = db.Recipes.Include(rp=>rp.RecipesProducts)
                        .FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
                    if (newRecipe == null)
                    {
                        return "";
                    }
                } 
                newRecipe.RecipeName = recipe.RecipeName;
                newRecipe.Steps = recipe.Steps;
                newRecipe.RecipeImg = recipe.RecipeImg;                
                var deletedIngredients = newRecipe.RecipesProducts
                    .Where(ip => !recipe.Products
                    .Any(ingr => ingr.RecipeProductId == ip.RecipeProductId))
                    .ToList();
                foreach (var deletedIngredient in deletedIngredients)
                {
                    db.RecipesProducts.Remove(deletedIngredient);
                }
                foreach (var ingr in recipe.Products)
                {
                    RecipesProducts ingredient;
                    if (ingr.RecipeProductId == 0)
                    {
                        ingredient = new RecipesProducts();
                        newRecipe.RecipesProducts.Add(ingredient);
                    }
                    else
                    {
                        ingredient = db.RecipesProducts
                            .FirstOrDefault(ip => ip.RecipeProductId == ingr.RecipeProductId);
                        if (ingredient == null)
                        {
                            continue;
                        }
                    }
                    ingredient.ProductId = ingr.ProductId;
                    ingredient.MeasureType = ingr.MeasureChar;
                    ingredient.MeasureNumber = ingr.MeasureNumber;
                    ingredient.Amount = (short?)(ingr.MeasureNumber 
                        * db.Products
                        .Find(ingr.ProductId)
                        .getMeasureAmount(ingr.MeasureChar));
                } 
                
                db.SaveChanges();
                return "";
            }  
        }


        [HttpGet("statRecipes")]
        public string getDailyRecipes(int userId, int daysAgo)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Include(x => x.DailyDiet).ThenInclude(dd => dd.Recipe).Where(u => u.UserId == userId).FirstOrDefault();
                List<RecipeStat> res = (from dd in user.DailyDiet.Where(dd => dd.Date == DateOnly.FromDateTime(DateTime.Today.AddDays(-daysAgo))) 
                                        select new RecipeStat { 
                                            Amount = dd.Amount, 
                                            RecipeId = dd.RecipeId, 
                                            RecipeImg = dd.Recipe.RecipeImg, 
                                            RecipeName = dd.Recipe.RecipeName })
                                            .ToList();
                return JsonConvert.SerializeObject(res);
            }
            
        }
    }
}
