using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ActivityCodeViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Activity Code")]
        [Required]
        public string ActivityCodeDesc { get; set; }
        
        [Display(Name = "Activity Owner")]
        [Required]
        public long? ActivityOwnerID { get; set; }
        [Display(Name = "Acitivity Owner")]
        public string ActivityOwnerFullName { get; set; }

        [Display(Name = "Accountable Manager")]
        [Required]
        public long? AccountableManagerID { get; set; }

        [Display(Name = "Accountable Manager")]
        public string AccountableManagerFullName { get; set; }

        [Display(Name = "Line Manager")]
        [Required]
        public long? LineManagerID { get; set; }
        public virtual AppUsers LineManager { get; set; }

        [Display(Name = "Line Manager")]
        public string LineManagerFullName { get; set; }

        public IEnumerable<ActivityCodeViewModel> lstActivityCodes { get; set; }

        public virtual ICollection<BudgetBook> BudgetBook { get; set; }

        [Display(Name = "Work Stream")]
        public long? ActivityCodeWorkStreamID { get; set; }
        public string WorkStreamName { get; set; }
        public int WorkFlowType { get; set; }
        public string WorkFlowTypeDesc { get; set; }
    }
}