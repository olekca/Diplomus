using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Recipes
    {
        public Recipes()
        {
            RecipesProducts = new HashSet<RecipesProducts>();
        }

        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = null!;
        public string[]? Steps { get; set; }
        public string[]? Ingredients { get; set; }

        public virtual ICollection<RecipesProducts> RecipesProducts { get; set; }
    }
}
