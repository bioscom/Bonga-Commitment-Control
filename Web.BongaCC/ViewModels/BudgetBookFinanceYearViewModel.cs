using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Web.BongaCC.ViewModels
{
    public class BudgetBookFinanceYearViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        public long? BudgetBookID { get; set; }

        [Display(Name = " BUDGET(S'NGN)")]
        [Required]
        public decimal OPYearBudgetNaira { get; set; }

        [Display(Name = " BUDGET(S'USD)")]
        [Required]
        public decimal OPYearBudgetDollar { get; set; }

        [Display(Name = " PLAN(F'USD)")]
        [Required]
        public decimal OPYearBudgetFDollar { get; set; }

        [Display(Name = " NAPIMS BUDGET(S'NGN)")]
        [Required]
        public decimal NAPIMSBUDGETNaira { get; set; }

        [Display(Name = " NAPIMS BUDGET(S'USD)")]
        [Required]
        public decimal NAPIMSBUDGETDollar { get; set; }

        [Display(Name = " NAPIMS BUDGET(F'USD)")]
        [Required]
        public decimal NAPIMSBUDGETFDollar { get; set; }

        [Display(Name = " Q1 FYLE(S'NGN)")]
        [Required]
        public decimal Q1FYLENaira { get; set; }

        [Display(Name = " Q1 FYLE(S'USD)")]
        [Required]
        public decimal Q1FYLEDollar { get; set; }

        [Display(Name = " Q1 FYLE(F'USD)")]
        [Required]
        public decimal Q1FYLEFDollar { get; set; }

        [Display(Name = " Q2 FYLE(S'NGN)")]
        [Required]
        public decimal Q2FYLENaira { get; set; }

        [Display(Name = " Q2 FYLE(S'USD)")]
        [Required]
        public decimal Q2FYLEDollar { get; set; }

        [Display(Name = " Q2 FYLE(F'USD)")]
        [Required]
        public decimal Q2FYLEFDollar { get; set; }

        [Display(Name = " Q3 FYLE(S'NGN)")]
        [Required]
        public decimal Q3FYLENaira { get; set; }

        [Display(Name = " Q3 FYLE(S'USD)")]
        [Required]
        public decimal Q3FYLEDollar { get; set; }

        [Display(Name = " Q3 FYLE(F'USD)")]
        [Required]
        public decimal Q3FYLEFDollar { get; set; }

        [Display(Name = " Q4 FYLE(S'NGN)")]
        [Required]
        public decimal Q4FYLENaira { get; set; }

        [Display(Name = " Q4 FYLE(S'USD)")]
        [Required]
        public decimal Q4FYLEDollar { get; set; }

        [Display(Name = " Q4 FYLE(F'USD)")]
        [Required]
        public decimal Q4FYLEFDollar { get; set; }

        [Display(Name = "Year")]
        public int YYear { get; set; }
    }

}
