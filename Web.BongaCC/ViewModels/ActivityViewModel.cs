using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.BongaCC.ViewModels
{
    public class ActivityViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        public IEnumerable<ActivityViewModel> lstActivities { get; set; }
    }
}