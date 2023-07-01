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

        public string getMeasureString(char? type)
        {
            switch (type)
            {
                case '1': return MeasureName1; break;
                case '2': return MeasureName2; break;
                default: return "grams"; break;
            }
        }

        public short getMeasureAmount(char? type)
        {
            switch (type)
            {
                case '1': return (short)MeasureAmount1; break;
                case '2': return (short)MeasureAmount2; break;
                default: return 1; break;
            }
        }
        public virtual ICollection<ProductsNutrients> ProductsNutrients { get; set; }
        public virtual ICollection<RecipesProducts> RecipesProducts { get; set; }
    }
}
