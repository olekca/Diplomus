using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class DailyDiet
    {
        public long DailyDietId { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public int RecipeId { get; set; }
        public short Amount { get; set; }

        public virtual Recipes Recipe { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
