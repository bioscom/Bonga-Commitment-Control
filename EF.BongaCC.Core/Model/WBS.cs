using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class WBS : BaseEntity
    {
        public string CostObjects { get; set; }
        public string CostObjectsDescription { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}