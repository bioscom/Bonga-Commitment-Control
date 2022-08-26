using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class ActivityDetailsViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public string Description { get; set; }
        //public decimal m_dAmount { get; set; } calculated

        [Display(Name = "Qty")]
        [Required]
        public decimal Quantity { get; set; }

        [Display(Name = "Rate")]
        [Required]
        public decimal Rate { get; set; }

        [Display(Name = "Fixed Rate")]
        public decimal? FixedExchangeRate { get; set; }


        [Display(Name = "Amount F($)")]
        public decimal? Calculated { get; set; }

        public int iYear { get; set; }

        //public long? CommitmentID { get; set; }

        public long? BudgetBookCommitmentsID { get; set; }
        public long? BudgetBookID { get; set; }
        public long? CapexOpexID { get; set; }

        [Display(Name = "Currency")]
        public long? CurrenciesID { get; set; }

        [Display(Name = "Currency")]
        public string CurrencyName { get; set; }
    }
}
