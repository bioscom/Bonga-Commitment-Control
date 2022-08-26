using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Scope : BaseEntity
    {
        public string Purpose { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
