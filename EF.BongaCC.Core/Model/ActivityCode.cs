using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class ActivityCode : BaseEntity
    {
        public string ActivityCodeDesc { get; set; }
        public virtual ICollection<BudgetBook> BudgetBook { get; set; }

        public long? ActivityOwnerID { get; set; }
        public long? AccountableManagerID { get; set; }
        public long? LineManagerID { get; set; }
        public virtual AppUsers LineManager { get; set; }
        public long? ActivityCodeWorkStreamID { get; set; }
        public virtual ActivityCodeWorkStream ActivityCodeWorkStream { get; set; }
    }
}
