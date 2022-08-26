using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class BudgetBaseViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Budget Base")]
        [Required]
        public string BudgetBase { get; set; }

        public IEnumerable<BudgetBaseViewModel> lstBudgetBases { get; set; }
    }
}