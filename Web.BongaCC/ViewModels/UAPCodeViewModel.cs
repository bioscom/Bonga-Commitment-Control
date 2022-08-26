using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.BongaCC.ViewModels
{
    public class UAPCodeViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Uap Code Desc")]
        [Required]
        public string UapCodeDesc { get; set; }

        public IEnumerable<UAPCodeViewModel> lstUapCodes { get; set; }
    }
}