using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class UapRollUpCode : BaseEntity
    {
        public string UapRollUpCodeDesc { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
