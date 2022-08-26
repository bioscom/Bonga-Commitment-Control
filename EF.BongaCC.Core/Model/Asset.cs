using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Asset : BaseEntity
    {
        public string AssetName { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }
    }
}
