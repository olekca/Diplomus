using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class RecipesProducts
    {
        public long RecipeProductId { get; set; }
        public int ProductId { get; set; }
        public int RecipeId { get; set; }
        public short? Amount { get; set; }
        public char? MeasureType { get; set; }
        public float? MeasureNumber { get; set; }

        public virtual Products Product { get; set; } = null!;
        public virtual Recipes Recipe { get; set; } = null!;
    }
}
