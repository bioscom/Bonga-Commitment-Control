using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ActivityTypeViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Activity Name")]
        [Required]
        public string ActivityName { get; set; }
        public IEnumerable<ActivityTypeViewModel> lstActivityTypes { get; set; }
    }
}