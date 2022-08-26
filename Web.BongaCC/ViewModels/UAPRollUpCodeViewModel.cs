using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.BongaCC.ViewModels
{
    public class UAPRollUpCodeViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "UAP Roll Up Code Desc")]
        [Required]
        public string UapRollUpCodeDesc { get; set; }

        public IEnumerable<UAPRollUpCodeViewModel> lstUapRollUpCodes { get; set; }
    }
}