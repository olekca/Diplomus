using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class NeedsNutrients
    {
        public int NeedNutrientId { get; set; }
        public int NeedId { get; set; }
        public short NutrientId { get; set; }
        public int? NutrientPercent { get; set; }

        public virtual Needs Need { get; set; } = null!;
        public virtual Nutrients Nutrient { get; set; } = null!;
    }
}
