using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Recipes
    {
        public Recipes()
        {
            DailyDiet = new HashSet<DailyDiet>();
            RecipesProducts = new HashSet<RecipesProducts>();
        }

        public int RecipeId { get; set; }

        public string RecipeName { get; set; } = null!;
        public string? RecipeImg { get; set; }
        public string[]? Steps { get; set; }
        public string[]? Ingredients { get; set; }
        

        public virtual ICollection<DailyDiet> DailyDiet { get; set; }
        public virtual ICollection<RecipesProducts> RecipesProducts { get; set; }
    }
}
