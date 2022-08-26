using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class ActivityType : BaseEntity
    {
        public string ActivityName { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}
