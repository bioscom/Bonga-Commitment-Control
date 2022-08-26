using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.BongaCC.ViewModels
{
    public class ScopesViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Purpose")]
        [Required]
        public string Purpose { get; set; }

        public IEnumerable<ScopesViewModel> lstScopes { get; set; }
    }
}