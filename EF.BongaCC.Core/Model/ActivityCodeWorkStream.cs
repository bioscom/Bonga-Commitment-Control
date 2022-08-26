using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class ActivityCodeWorkStream : BaseEntity
    {
        public string WorkStream { get; set; }
        public string WorkStreamDesc { get; set; }
        public int WorkFlowType { get; set; }
        public virtual ICollection<ActivityCode> ActivityCodes { get; set; }
    }
}
