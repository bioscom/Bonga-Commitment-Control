using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class CXOX : BaseEntity
    {
        public string CapexOpex { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }

    }
}
