using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class CurrenciesViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }
        public string Codes { get; set; }
        public int Number { get; set; }
    }
}
