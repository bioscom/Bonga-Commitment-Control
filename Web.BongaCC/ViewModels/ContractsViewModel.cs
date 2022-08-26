using EF.BongaCC.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ContractsViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Contract Name")]
        [Required]
        public string ContractName { get; set; }

        public IEnumerable<ContractsViewModel> lstContracts { get; set; }
    }
}