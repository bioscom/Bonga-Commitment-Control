using System.Collections.Generic;

/// <summary>
/// Summary description for PurchasingGroup
/// </summary>

namespace EF.BongaCC.Core.Model
{
    public class Team : BaseEntity
    {
        public string TeamName { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }

        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }
    }
}