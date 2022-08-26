using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }
        public virtual ICollection<Commitments> Commitments { get;set;}
    }
}
