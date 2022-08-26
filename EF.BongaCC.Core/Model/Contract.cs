using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Contract : BaseEntity
    {
        public string ContractName { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
