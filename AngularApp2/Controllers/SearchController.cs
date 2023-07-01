using AngularApp2.Models.Entity;
using AngularApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using System.Linq;
using System.Data.Entity.Core.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AngularApp2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public SearchController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("Products")]
        public string GetProducts(string searchLine)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
               List<ProductShort> products = _mapper.Map<List<ProductShort>>(
                   db.Products
                   .Where(p => p.ProductName.ToUpper()
                   .Contains(searchLine.ToUpper())));
                return JsonConvert.SerializeObject(products);
            }
        }

        [HttpGet("ProductMeasure")]
        public string GetProductMeasure(int ProductId)
        {
            List<DropDownValues> res = new List<DropDownValues> { new DropDownValues('0', "grams") };
            using (DiplomusContext db = new DiplomusContext())
            {
                Products product = db.Products.Find(ProductId);
                if (product.MeasureName1 != "")
                {
                    res.Add(new DropDownValues('1', product.MeasureName1));
                }
                if (product.MeasureName2 != "")
                {
                    res.Add(new DropDownValues('2', product.MeasureName2));
                }

                return JsonConvert.SerializeObject(res);
            }
        }

        [HttpGet("Needs")]
        public string GetNeeds(string searchLine)
       {
            if (searchLine == null)
            {
                searchLine = "";
            }
            using (DiplomusContext db = new DiplomusContext())
            {
                List<NeedShort> products = _mapper.Map<List<NeedShort>>(db.Needs.Where(n => n.NeedsName.ToUpper().Contains(searchLine.ToUpper())));
                return JsonConvert.SerializeObject(products);
            }
        }

        [HttpPost("SearchByRequest")]
        public string searchRecipes(SearchRequest request)  
        {
            int[] CoefAr = getNutrientArray(100);
            SearchResult res = new SearchResult();
            using (DiplomusContext db = new DiplomusContext())
            {                
                int[] excludedProducts = ExcludeId(request.ExcludedProducts);
                int[] includedProducts = ExcludeId(request.IncludedProducts);
                
                foreach (NeedShort n in request.Needs)
                {
                    Needs need = db.Needs.
                        Include(need=>need.NeedsNutrients)
                        .Where (need=>need.NeedsId==n.NeedsId)
                        .SingleOrDefault();
                    foreach(NeedsNutrients nn in need.NeedsNutrients)
                    {
                        CoefAr[nn.NutrientId - 1] += (int)nn.NutrientPercent!;
                    }
                    excludedProducts = excludedProducts
                        .Union(need.ExcludedProducts)
                        .ToArray();
                }
                 
                var recipes = db.Recipes
                    .Include(r=>r.RecipesProducts)
                    .ThenInclude(rp=>rp.Product)
                    .ThenInclude(p=>p.ProductsNutrients)
                    .AsQueryable();
                if (excludedProducts!=null && excludedProducts.Length != 0)
                {
                    recipes = recipes.Where(r => r.RecipesProducts.All(rp => !excludedProducts.Contains(rp.ProductId)));
                }
                if (includedProducts!=null && includedProducts.Length != 0)
                {
                    recipes = recipes.Where(r => r.RecipesProducts.All(rp => includedProducts.Contains(rp.ProductId)));
                }
                if (request.SearchLine != null)
                {
                    recipes = recipes
                    .Where(r => r.RecipeName.ToUpper().Contains( request.SearchLine.ToUpper()));
                }

                res.RecipesNum = recipes.Count();

                
                Recipes[] recipes1 = recipes.ToArray();
                RecipeShort[] temp = new RecipeShort[recipes1.Length];
                for (int i = 0; i<recipes1.Length; i++)
                {
                    temp[i] = _mapper.Map<RecipeShort>(recipes1[i]);
                    temp[i].Coef = getRecipeValue(recipes1[i], CoefAr);
                }
                temp = temp.OrderByDescending(r => r.Coef).ToArray();

                /* res.Recipes = (from rec in recipes
                                               select new RecipeShort {
                                                   RecipeId = rec.RecipeId,
                                                   RecipeImg = rec.RecipeImg,
                                                   RecipeName = rec.RecipeName,
                                                   Coef = getRecipeValue(rec, CoefAr)
                                               })
                                               .OrderBy(r=>r.Coef)
                                               .Skip((request.Page - 1) * request.RecipesPerPage)
                                               .Take(request.RecipesPerPage)
                                               .ToArray(); */

                res.Recipes = temp;
                res.MaxPage = (int)Math.Ceiling(res.RecipesNum / (double)request.RecipesPerPage);
                if (request.Page > res.MaxPage)
                {
                    res.CurPage = res.MaxPage;
                }
                else
                {
                    res.CurPage = request.Page;
                }
                return JsonConvert.SerializeObject(res);
            }
            
        }

        [HttpGet("AllRecipes")]
        public string getRecipes(int page, int recipesPerPage)
        {
            SearchResult res = new SearchResult();
            using (DiplomusContext db = new DiplomusContext())
            {
                Recipes[] nextPage = db.Recipes
                .OrderBy(r => r.RecipeId)
                .Skip((page - 1) * recipesPerPage)
                .Take(recipesPerPage)
                .ToArray();
                res.Recipes = _mapper.Map <RecipeShort[] >(nextPage);
                res.RecipesNum = db.Recipes.Count();
                res.MaxPage = (int)Math.Ceiling( res.RecipesNum/ (double)recipesPerPage);
                if (page > res.MaxPage)
                {
                    res.CurPage = res.MaxPage;
                }
                else
                {
                    res.CurPage = page;
                }
                return JsonConvert.SerializeObject(res);
            }
        }

        public static int[] ExcludeId(ProductShort[] ar)
        {
            int[] res = new int[ar.Length];
            for (int i = 0; i<ar.Length; ++i)
            {
                res[i] = ar[i].ProductId;
            }
            return res;
        }
        public static int[] ExcludeId(NeedShort[] ar)
        {
            int[] res = new int[ar.Length];
            for (int i = 0; i < ar.Length; ++i)
            {
                res[i] = ar[i].NeedsId;
            }
            return res;
        }

        [HttpGet("recipeValue")]
        public string GetRecipeNutrition(int recipeId)
        {
            using (DiplomusContext db= new DiplomusContext())
            {
                short[] nutrientCoefficients = new short[41];
                var recipe = db.Recipes.Include(r => r.RecipesProducts).ThenInclude(rp => rp.Product).ThenInclude(p => p.ProductsNutrients)
                .Where(r => r.RecipeId == recipeId).FirstOrDefault();
                string recipeTotalNutrientSum = (from rp in recipe.RecipesProducts
                                              select (from p in rp.Product.ProductsNutrients select p.NutrientPercent).Sum()).Sum() +"";

                return "" +recipeTotalNutrientSum;
            }

          
        }

        [HttpGet("productValue")]
        public string GetProductValue(int productId, int Coef)
        {
            int[] CoefAr = getNutrientArray(Coef);
            using (DiplomusContext db = new DiplomusContext())
            {
               Products temp = db.Products
                    .Include(p => p.ProductsNutrients)
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefault();
                string res = (from pn in temp.ProductsNutrients
                              select pn.NutrientPercent * CoefAr[pn.NutrientId - 1])
                              .Sum() + "";
                return res;
            }
        }

        private int getRecipeValue(Recipes recipe, int[] nutrientAr)
        {
            int fullAmount = (int)recipe.RecipesProducts.Sum(rp=>rp.Amount);
            int res = 0;
            foreach(RecipesProducts rp in recipe.RecipesProducts) 
            {
                res += (int)(getProdValue(rp.Product, nutrientAr) * (rp.Amount / (double)fullAmount));
            }           
            return res;
        }

        private int getProdValue(Products prod, int[] nutrientAr)
        {
            int res = 0;
            
            foreach (ProductsNutrients pn in prod.ProductsNutrients)
            {
                res+= (int)(pn.NutrientPercent* nutrientAr[pn.NutrientId - 1]);
            }           
            return res;
        }

        public static int[] getNutrientArray(int value = 0)//all nutrients have id from 1 to 41,
                                                           //so we need -1 when working with arr
        {
            int[] res = new int[41];
            for (int i =0; i<res.Length; ++i)
            {
                res[i] = value;
            }
            return res;
        }

        [HttpGet("needDesc")]
        public string getNeedDesc(int needId)
        {
            using (DiplomusContext db= new DiplomusContext())
            {
                Needs n = db.Needs.Find(needId);
                NeedDTO res = _mapper.Map<NeedDTO>(n);
                return JsonConvert.SerializeObject(res);
            }
        }

        [HttpGet("userNeeds")]
        public string getUserNeeds(int userId)
        {
            
            using (DiplomusContext db = new DiplomusContext())
            {
                List<NeedDTO> res = new List<NeedDTO>();
                Users user = db.Users.Find(userId);
                foreach(int i in user.Needs)
                {
                    Needs n = db.Needs.Find(i);
                   res.Add(_mapper.Map<NeedDTO>(n));
                }
                return JsonConvert.SerializeObject(res);
            }
        }
    }
}
