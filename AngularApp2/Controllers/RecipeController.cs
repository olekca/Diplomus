using AngularApp2.Models.Entity;
using AngularApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AngularApp2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RecipeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*   [HttpGet("login")]
           public string Login(string email, string password)//tested a little
           {
               LoginDTO res = new LoginDTO();
               using (DiplomusContext db = new DiplomusContext())
               {
                   Users user = db.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                   if (user != null)
                   {
                       res.UserAuthorized(user);
                   }
               }
               return JsonConvert.SerializeObject(res);
           }
        var temp = from b in db.Books
                       orderby b.Author
                       select new BookDTO
                       {
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           ReviewsNumber = b.Reviews.Count(),
                           Rating = Convert.ToDecimal((from r in b.Ratings
                                                       select r.Score).Average())
                       };
                List<BookDTO> res = temp.ToList<BookDTO>();
         
         
         */

        [HttpGet("page")]
        public string getRecipes(int lastIndex)
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

        [HttpGet]
        public string getRecipe(int recipeId)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Recipes recipe = db.Recipes.Where(r => r.RecipeId ==recipeId).FirstOrDefault();
                return JsonConvert.SerializeObject(recipe);
            }
            
        }

    }
}
