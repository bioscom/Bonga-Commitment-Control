using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class WorkFlow : BaseEntity
    {
        public long sponsorID { get; set; }
        public int checkedbyID { get; set; }
        public int approverID { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }
    }
}
