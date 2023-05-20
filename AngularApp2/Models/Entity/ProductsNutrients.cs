using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class ProductsNutrients
    {
        public int ProductNutrientId { get; set; }
        public int ProductId { get; set; }
        public short NutrientId { get; set; }
        public float NutrientAmount { get; set; }
        public short? NutrientPercent { get; set; }

        public virtual Nutrients Nutrient { get; set; } = null!;
        public virtual Products Product { get; set; } = null!;
    }
}
