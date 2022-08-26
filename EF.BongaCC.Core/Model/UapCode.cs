using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class UapCode : BaseEntity
    {
        public string UapCodeDesc { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
