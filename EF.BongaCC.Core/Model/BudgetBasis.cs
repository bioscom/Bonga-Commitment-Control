using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class BudgetBasis : BaseEntity
    {
        public string BudgetBase { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
