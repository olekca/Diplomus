using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Nutrients
    {
        public Nutrients()
        {
            NeedsNutrients = new HashSet<NeedsNutrients>();
            ProductsNutrients = new HashSet<ProductsNutrients>();
        }

        public short NutrientId { get; set; }
        public string NutrientNameUa { get; set; } = null!;
        public float? DailyDose { get; set; }
        public string? Description { get; set; }
        public bool? IsHydrophobic { get; set; }
        public bool? IsThermophobic { get; set; }
        public string? NutrientName { get; set; }
        public string? DoseMeasure { get; set; }

        public virtual ICollection<NeedsNutrients> NeedsNutrients { get; set; }
        public virtual ICollection<ProductsNutrients> ProductsNutrients { get; set; }
    }
}
