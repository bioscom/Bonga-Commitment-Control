using System.Collections.Generic;

namespace EF.BongaCC.Core.Model
{
    public class Area : BaseEntity
    {
        public string AreaName { get; set; }
        public virtual ICollection<Facility> Facilities { get; set; }
        //public virtual ICollection<Commitments> Commitments { get; set; }
    }
}
