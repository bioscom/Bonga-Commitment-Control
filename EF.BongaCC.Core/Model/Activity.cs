using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Activity : BaseEntity
    {
        public string Description { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
