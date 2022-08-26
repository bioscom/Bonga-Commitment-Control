using System.Collections.Generic;

namespace EF.BongaCC.Core.Model
{
    public class Facility : BaseEntity
    {
        public string FacilityName { get; set; }
        public string Code { get; set; }
        public int Agg { get; set; }
        public int Location { get; set; }

        public long AreaID { get; set; }
        public virtual Area Area { get; set; }

        public virtual ICollection<Commitments> Commitments { get; set; }
    }
}
