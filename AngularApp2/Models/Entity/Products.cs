using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Products
    {
        public Products()
        {
            ProductsNutrients = new HashSet<ProductsNutrients>();
            RecipesProducts = new HashSet<RecipesProducts>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Category { get; set; }
        public string? MeasureName1 { get; set; }
        public short? MeasureAmount1 { get; set; }
        public string? MeasureName2 { get; set; }
        public short? MeasureAmount2 { get; set; }

        public virtual ICollection<ProductsNutrients> ProductsNutrients { get; set; }
        public virtual ICollection<RecipesProducts> RecipesProducts { get; set; }
    }
}
