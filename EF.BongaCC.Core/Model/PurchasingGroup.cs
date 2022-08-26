using System.Collections.Generic;

/// <summary>
/// Summary description for PurchasingGroup
/// </summary>

namespace EF.BongaCC.Core.Model
{
    public class PurchasingGroup : BaseEntity
    {
        public string GroupName { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }
        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }

    }
}