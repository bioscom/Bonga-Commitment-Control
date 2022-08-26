using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ExchangeRatesViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Floating Exchange Rate")]
        [Required]
        public decimal FloatingExchangeRate { get; set; }

        [Display(Name = "Month")]
        public int MMonth { get; set; }

        [Display(Name = "Month")]
        public string sMonth { get; set; }

        [Display(Name = "Year")]
        public int YYear { get; set; }

        [Display(Name = "Day")]
        public int iDay { get; set; }

        public IEnumerable<ExchangeRatesViewModel> lstExchangeRates { get; set; }
    }
}
