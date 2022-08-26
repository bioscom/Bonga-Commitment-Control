using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ActivityCodeWorkStreamViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Work Stream")]
        [Required]
        public string WorkStream { get; set; }

        [Display(Name = "Work Stream Description")]
        public string WorkStreamDesc { get; set; }

        [Display(Name = "Work Workflow Type")]
        public int WorkFlowType { get; set; }
        public string sWorkFlowType { get; set; }

        public IEnumerable<ActivityCodeWorkStreamViewModel> lstActivityCodesWS { get; set; }
        public IEnumerable<ActivityCodeViewModel> lstActivityCodes { get; set; }
    }
}