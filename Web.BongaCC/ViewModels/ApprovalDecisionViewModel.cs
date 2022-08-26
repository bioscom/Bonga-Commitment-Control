using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ApprovalDecisionsViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Approval Decision")]
        [Required]
        public string Decision { get; set; }

        [Display(Name = "Color Code")]
        public string ColorCode { get; set; }


        public IEnumerable<ApprovalDecisionsViewModel> lstApprovalDecisions { get; set; }

        //public virtual ICollection<BudgetBook> BudgetBook { get; set; }
    }
}