using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class BudgetUploader : BaseEntity
    {
        public string ActivityType { get; set; }
        public string DirectAllocated { get; set; }
        public string UapCode { get; set; }
        public string UapRollUpCode { get; set; }
        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }
        public string LineManager { get; set; }

        public string CostCenter { get; set; }
        public string Activity { get; set; }
        public string ActivityOwner { get; set; }
        
        public string AccountableManager { get; set; }
        public string ScopePurpose { get; set; }
        public string Contract { get; set; }
        public string Budgetbasis { get; set; }
        public decimal OPYearBudget { get; set; }
        public int YYear { get; set; }
    }
}
