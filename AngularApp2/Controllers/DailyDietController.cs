using AngularApp2.Models;
using AngularApp2.Models.Entity;
using AutoMapper;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace AngularApp2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DailyDietController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DailyDietController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("addStat")]
        public IActionResult AddStatistics(int UserId, int RecipeId, short Amount)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Include(u => u.DailyDiet).Where(u => u.UserId == UserId).SingleOrDefault();
                DailyDiet diet = new DailyDiet();
                diet.RecipeId = RecipeId;
                diet.Amount = Amount;
                diet.Date = DateOnly.FromDateTime(DateTime.Today);
                user.DailyDiet.Add(diet);
                db.SaveChanges();
                return Ok();
            }
        }


        [HttpGet("DummyStat")]
        
        public string GetDummyStat(int recipeId)
        {
            StatNutrients[] stat = new StatNutrients[35];
            for (int i = 0; i<39; ++i)
            {
                stat[i] = new StatNutrients
                {
                    NutrientId = (short)i,
                    NutrientName = "Nutrient " + i,
                    DailyDose = (short)100 + i,
                    Color = getColor(i),
                    NutrientPercent = (short)(i * 3),
                    DoseMeasure = "mg",
                    NutrientAmount  = (short)(i/2)
                    
                };
            }
            return JsonConvert.SerializeObject(stat);
        }

        private StatNutrients[] getEmptyStat()
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                StatNutrients[] res = (from n in db.Nutrients orderby n.NutrientId
                                       select new StatNutrients
                                       {
                                           NutrientId = n.NutrientId,
                                           NutrientName = n.NutrientName,
                                           DailyDose = n.DailyDose,
                                           NutrientPercent = 0,
                                           DoseMeasure = n.DoseMeasure,
                                           Color = getColor(0),
                                           NutrientAmount = 0
                                       }).ToArray();
                return res;
            }
        }

        [HttpGet("ProductStat")]

        public string GetProductStat(int productId) {
            using (DiplomusContext db = new DiplomusContext())
            {
                Products prod = db.Products.Include(pn=>pn.ProductsNutrients).ThenInclude(n=>n.Nutrient).Where(p=>p.ProductId== productId).FirstOrDefault();
                StatNutrients[] res = (from pn in prod.ProductsNutrients
                                       select new StatNutrients
                                       {
                                           NutrientId = pn.NutrientId,
                                           NutrientName = pn.Nutrient.NutrientName,
                                           DailyDose = pn.Nutrient.DailyDose,
                                           NutrientPercent = (int)pn.NutrientPercent,
                                           DoseMeasure = pn.Nutrient.DoseMeasure,
                                           Color = getColor((int)pn.NutrientPercent),
                                           NutrientAmount = pn.NutrientAmount
                                       }).ToArray();

                return JsonConvert.SerializeObject(res);
            }

            

        }

        [HttpGet("RecipeStat")]
        public string GetRecipeStat(int recipeId)
        {
            using  (DiplomusContext db = new DiplomusContext())
            {
                Recipes recipe = db.Recipes
                    .Include(rp => rp.RecipesProducts)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(pn => pn.ProductsNutrients)
                    .ThenInclude(n => n.Nutrient)
                    .Where(r => r.RecipeId == recipeId)
                    .FirstOrDefault();

                StatNutrients[] stats = RecipeStat(recipe);
                stats = deleteTechNutrients(stats);
                return JsonConvert.SerializeObject(stats);
            }
        }

        [HttpGet("StatPerDay")]
        public string GetUserStat(int userId, int daysAgo)
        {
            StatNutrients[] res;
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Include(u=>u.DailyDiet).ThenInclude(dd=>dd.Recipe)
                    .ThenInclude(rp => rp.RecipesProducts)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(pn => pn.ProductsNutrients)
                    .ThenInclude(n => n.Nutrient)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault();
                res = UserDailyStat(user, daysAgo);
                res = deleteTechNutrients(res);
                return JsonConvert.SerializeObject(res);
            }
            
        }

        [HttpGet("StatPerPeriod")]

        public string GetPeriodStat(int userId, int days)
        {
            List<StatNutrients[]> ar = new List<StatNutrients[]>();
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Include(u => u.DailyDiet).ThenInclude(dd => dd.Recipe)
                    .ThenInclude(rp => rp.RecipesProducts)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(pn => pn.ProductsNutrients)
                    .ThenInclude(n => n.Nutrient)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault();

                for (int i = 0; i<days; ++i)
                {
                    ar.Add(UserDailyStat(user, i));
                }
                StatNutrients[] res = ToAveragePerPeriod(ar);
                res = deleteTechNutrients(res);
                return JsonConvert.SerializeObject(res);
            }
        }

        
        private StatNutrients[] UserDailyStat(Users user, int daysAgo = 0)
        {
            List<DailyDiet> dailyDiet = user.DailyDiet.Where(dd => dd.Date == DateOnly.FromDateTime(DateTime.Today.AddDays(-daysAgo))).ToList();
            if (dailyDiet.Count() == 0)
            {
                return getEmptyStat();
            }
            List<StatNutrients[]> list = new List<StatNutrients[]>();
            short[] coefsArray = new short[dailyDiet.Count()];
            int i = 0;
            foreach (DailyDiet dd in dailyDiet)
            {
                coefsArray[i] = (short)dd.Amount; i++;
                StatNutrients[] res = RecipeStat(dd.Recipe);
                list.Add(res);
            }
            StatNutrients[] stats = ToAveragePerDay(list, coefsArray);
            return stats;
        }

        private StatNutrients[] RecipeStat(Recipes recipe)
        {
            List<StatNutrients[]> list = new List<StatNutrients[]>();
            if (recipe.RecipesProducts.Count == 0)
            {
                return getEmptyStat();
            }
            short[] coefsArray = new short[recipe.RecipesProducts.Count];
            int i = 0;
            foreach (RecipesProducts recProd in recipe.RecipesProducts)
            {
                StatNutrients[] res = (from pn in recProd.Product.ProductsNutrients
                                       select new StatNutrients
                                       {
                                           NutrientId = pn.NutrientId,
                                           NutrientName = pn.Nutrient.NutrientName,
                                           DailyDose = pn.Nutrient.DailyDose,
                                           NutrientPercent = (int)pn.NutrientPercent,
                                           DoseMeasure = pn.Nutrient.DoseMeasure,
                                           Color = getColor((int)pn.NutrientPercent),
                                           NutrientAmount = pn.NutrientAmount
                                       }).Where(s => s.DailyDose != null).ToArray();
                coefsArray[i] = (short)recProd.Amount; i++;


                list.Add(res);
            }
            StatNutrients[] stats = ToAveragePerRecipe(list, coefsArray);
            return stats;
        }

        private StatNutrients[] ToAveragePerRecipe(List<StatNutrients[]> list, short[] amountCoef)
        {
            if (list.Count() == 0)
            {
                return getEmptyStat();
            }
            StatNutrients[] res = new StatNutrients[list[0].Length];
            for (int i = 0; i < list[0].Length; ++i)
            {
                res[i] = new StatNutrients();
                res[i].NutrientId = list[0][i].NutrientId;
                res[i].NutrientName = list[0][i].NutrientName;
                res[i].DailyDose = list[0][i].DailyDose;
                res[i].DoseMeasure = list[0][i].DoseMeasure;
                float AmountSum = amountCoef.Sum(a=>a);
                for (int j = 0; j<amountCoef.Length; ++j)
                {
                    
                    res[i].NutrientAmount += (list[j][i].NutrientAmount * (amountCoef[j]/AmountSum));
                }
                res[i].NutrientPercent = (res[i].NutrientAmount / res[i].DailyDose)*100;
                res[i].Color = getColor((int)res[i].NutrientPercent);
            }
            return res;
        }

        private StatNutrients[] ToAveragePerDay(List<StatNutrients[]> list, short[] amountCoef)
        {
            if (list.Count() == 0)
            {
                return getEmptyStat();
            }
            StatNutrients[] res = new StatNutrients[list[0].Length];
            for (int i = 0; i < list[0].Length; ++i)
            {
                res[i] = new StatNutrients();
                res[i].NutrientId = list[0][i].NutrientId;
                res[i].NutrientName = list[0][i].NutrientName;
                res[i].DailyDose = list[0][i].DailyDose;
                res[i].DoseMeasure = list[0][i].DoseMeasure;
                
                for (int j = 0; j < amountCoef.Length; ++j)
                {

                    res[i].NutrientAmount += (list[j][i].NutrientAmount * (amountCoef[j] /(float)100) ); 
                }

                res[i].NutrientPercent =(int) ((res[i].NutrientAmount / res[i].DailyDose) * 100);
                res[i].Color = getColor((int)res[i].NutrientPercent);



            }
            return res;
        }

        private StatNutrients[] ToAveragePerPeriod(List<StatNutrients[]> list)
        {
            if (list.Count() == 0)
            {
                return getEmptyStat();
            }
            StatNutrients[] res = new StatNutrients[list[0].Length];
            for (int i = 0; i < list[0].Length; ++i)
            {
                res[i] = new StatNutrients();
                res[i].NutrientId = list[0][i].NutrientId;
                res[i].NutrientName = list[0][i].NutrientName;
                res[i].DailyDose = list[0][i].DailyDose!;
                if (res[i].DailyDose == null) { res[i].DailyDose = 1; }
                res[i].DoseMeasure = list[0][i].DoseMeasure;
                
                for (int j = 0; j < list.Count(); ++j)
                {
                    if (list[j].Length <=i)
                    {
                        continue;
                    }
                    res[i].NutrientAmount += list[j][i].NutrientAmount;
                }
                res[i].NutrientAmount /= list.Count();
                res[i].NutrientPercent =(int) ((res[i].NutrientAmount / res[i].DailyDose) * 100);
                res[i].Color = getColor((int)res[i].NutrientPercent);
            }
            return res;
        }

        private string getColor(int percent)
        {
            if (percent < 10)
            {
                return "red";
            }
            else if (percent < 25)
            {
                return "Orange";

            }
            else if (percent < 50)
            {
                return "Gold";

            }
            else if (percent < 75)
            {
                return "GreenYellow";

            }
            else if (percent < 100)
            {
                return "Green";
            }
            else
            {
                return "MediumSlateBlue";
            }
        }
        /*
         public class StatNutrients
    {
        public short NutrientId { get; set; } 
        public string? NutrientName { get; set; }
        public float? DailyDose { get; set; }
        public string Color { get; set; }
        public double? NutrientPercent { get; set; }     
       
        public string? DoseMeasure { get; set; }
        public float NutrientAmount { get; set; }
    }
         */

        public string GetRecipeStatistics(int recipeId)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Recipes recipe = db.Recipes.Include(r => r.RecipesProducts).ThenInclude(rp => rp.Product).ThenInclude(p=>p.ProductsNutrients).ThenInclude(n=>n.Nutrient).FirstOrDefault(r => r.RecipeId == recipeId);

                StatNutrients[] nutrients = (from prod in recipe.RecipesProducts 
                                             select new StatNutrients { 
                                                 
                                                 
                }).ToArray();
            }

           /* RecipeProds[] i = (from prod in recipe.RecipesProducts
                               select new RecipeProds
                               {
                                   MeasureNumber = prod.MeasureNumber,
                                   MeasureType = prod.Product.getMeasureString(prod.MeasureType),
                                   MeasureChar = prod.MeasureType,
                                   ProductId = prod.ProductId,
                                   ProductName = prod.Product.ProductName,
                                   RecipeProductId = prod.RecipeProductId
                               }).ToArray();*/

            return "";
        }

        public void SetAmount()
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                RecipesProducts[] rp = db.RecipesProducts.Include(rp => rp.Product).ToArray();
                foreach(RecipesProducts rec in rp)
                {
                    rec.Amount = (short)(rec.MeasureNumber * rec.Product.getMeasureAmount(rec.MeasureType));
                }
                db.SaveChanges();
            }
        }

        private StatNutrients[] deleteTechNutrients(StatNutrients[] ar)
        {
            List<StatNutrients> res = new List<StatNutrients>();
            foreach(StatNutrients n in ar)
            {
                if (n.DailyDose==0 || n.DailyDose == 1)
                {
                    continue;
                }
                else
                {
                    res.Add(n);
                }
            }
            return res.ToArray();
            
        }

    }
}
