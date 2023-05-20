using System;
using System.Collections.Generic;

namespace AngularApp2.Models.Entity
{
    public partial class Needs
    {
        public Needs()
        {
            NeedsNutrients = new HashSet<NeedsNutrients>();
        }

        public int NeedsId { get; set; }
        public string NeedsName { get; set; } = null!;
        public string? Desc { get; set; }
        public int[]? ExcludedProducts { get; set; }

        public virtual ICollection<NeedsNutrients> NeedsNutrients { get; set; }
    }
}
