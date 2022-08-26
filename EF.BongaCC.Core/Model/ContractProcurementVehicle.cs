using System.Collections.Generic;

/// <summary>
/// Summary description for ContractProcurementVehicle
/// </summary>
/// 

namespace EF.BongaCC.Core.Model
{
    public class ContractProcurementVehicle : BaseEntity
    {
        public string VehicleName { get; set; }
        public virtual ICollection<Commitments> Commitments { get; set; }
        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }
    }
}