using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class WBSViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Cost Object")]
        [Required]
        public string CostObjects { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string CostObjectsDescription { get; set; }

        public IEnumerable<WBSViewModel> lstWBS { get; set; }

    }
}