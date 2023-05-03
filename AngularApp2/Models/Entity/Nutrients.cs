using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Nutrients
    {
        public short NutrientId { get; set; }
        public string NutrientNameUa { get; set; } = null!;
        public float? DailyDose { get; set; }
        public string? Description { get; set; }
        public bool? IsHydrophobic { get; set; }
        public bool? IsThermophobic { get; set; }
        public string? NutrientName { get; set; }
    }
}
