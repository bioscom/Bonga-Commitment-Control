using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ApprovalDecision
/// </summary>
/// 
namespace EF.BongaCC.Core.Model
{
    public class ApprovalDecision : BaseEntity
    {
        public string Decision { get; set; }
        public string ColorCode { get; set; }

        public virtual ICollection<Commitments> Commitments { get; set; }
        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }
        
    }
}