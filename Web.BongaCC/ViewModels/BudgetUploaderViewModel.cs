using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class BudgetUploaderViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }
        public string ActivityType { get; set; }
        public string DirectAllocated { get; set; }
        public string UapCode { get; set; }
        public string UapRollUpCode { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string CostCenter { get; set; }
        public string Activity { get; set; }
        public string ActivityOwner { get; set; }
        public string LineManager { get; set; }
        public string AccountableManager { get; set; }
        public string ScopePurpose { get; set; }
        public string Contract { get; set; }
        public string Budgetbasis { get; set; }
        public decimal OPYearBudget { get; set; }
        public int? YYear { get; set; }

        //[Display(Name = "Activity Code")]
        //[Required]
        //public string ActivityCodeDesc { get; set; }

        //[Display(Name = "Line Manager")]
        //[Required]
        //public long? LineManagerID { get; set; }
        //public virtual AppUsers LineManager { get; set; }

        //[Display(Name = "Line Manager")]
        //public string LineManagerFullName { get; set; }

        //public IEnumerable<ActivityCodeViewModel> lstActivityCodes { get; set; }
    }
}
