using System.Collections.Generic;

/// <summary>
/// Summary description for PlannedEmmergency
/// </summary>
/// 
namespace EF.BongaCC.Core.Model
{
    public class PlannedEmmergency : BaseEntity
    {
        public string PlanEmmerType { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }
        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }

    }
}
