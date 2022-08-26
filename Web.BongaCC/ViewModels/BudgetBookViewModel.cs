using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Web.BongaCC.ViewModels
{
    public class BudgetBookViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        [Display(Name = "Capex / Opex")]
        [EnumDataType(typeof(enuCapexOpex))]
        [Required]
        public long CapexOpexID { get; set; }

        [Display(Name = "Capex / Opex")]
        public string CapexOpex { get; set; }

        [Display(Name = "Direct / Allocated")]
        [EnumDataType(typeof(enuDirectAllocated))]
        [Required]
        public int DirectAllocated { get; set; }

        [Display(Name = "Direct / Allocated")]
        public string sDirectAllocated { get; set; }

        [Display(Name = "UAP Code")]
        [Required]
        public long UapCodeID { get; set; }

        [Display(Name = "UAP Code")]
        public string UapCode { get; set; }

        [Display(Name = "UAP Roll Up Code")]
        [Required]
        public long UapRollUpCodeID { get; set; }

        [Display(Name = "UAP Roll Up Code")]
        public string UapRollUpCode { get; set; }

        [Display(Name = "Activity Name")]
        [Required]
        public long ActivityTypeID { get; set; }

        [Display(Name = "Activity Name")]
        public string ActivityType { get; set; }

        [Display(Name = "Activity Code")]
        [Required]
        public long ActivityCodeID { get; set; }

        [Display(Name = "Activity Code")]
        public string ActivityCode { get; set; }

        [Display(Name = "Cost Object")]
        [Required]
        public long wbsID { get; set; }

        [Display(Name = "Cost Object")]
        public string CostObject { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public long ActivityID { get; set; }

        [Display(Name = "Activity")]
        public string Activity { get; set; }

        [Display(Name = "Activity Owner")]
        [Required]
        public long? ActivityOwnerID { get; set; }

        [Display(Name = "Activity Owner")]
        public string ActivityOwner { get; set; }

        [Display(Name = "Line Manager")]
        public long? LineManagerID { get; set; }

        [Display(Name = "Line Manager")]
        public string LineManager { get; set; }

        [Display(Name = "Accountable Manager")]
        [Required]
        public long? SponsorID { get; set; }

        [Display(Name = "Accountable Manager")]
        public string Sponsor { get; set; }

        [Display(Name = "Scope")]
        [Required]
        public long ScopeID { get; set; }

        [Display(Name = "Scope")]
        public string Scope { get; set; }

        [Display(Name = "Contract")]
        [Required]
        public long ContractID { get; set; }

        [Display(Name = "Contract")]
        public string Contract { get; set; }

        [Display(Name = "Budget Basis")]
        [Required]
        public long BudgetBasisID { get; set; }

        [Display(Name = "Budget Basis")]
        public string BudgetBasis { get; set; }


        [Display(Name = " BUDGET(S'NGN)" )]
        [Required]
        public decimal OPYearBudgetNaira { get; set; }

        [Display(Name = " BUDGET(S'USD)" )]
        [Required]
        public decimal OPYearBudgetDollar { get; set; }

        [Display(Name = " PLAN(F'USD)" )]
        [Required]
        public decimal OPYearBudgetFDollar { get; set; }

        [Display(Name = " NAPIMS BUDGET(S'NGN)" )]
        [Required]
        public decimal NAPIMSBUDGETNaira { get; set; }

        [Display(Name = " NAPIMS BUDGET(S'USD)" )]
        [Required]
        public decimal NAPIMSBUDGETDollar { get; set; }

        [Display(Name = " NAPIMS BUDGET(F'USD)" )]
        [Required]
        public decimal NAPIMSBUDGETFDollar { get; set; }

        [Display(Name = " Q1 FYLE(S'NGN)" )]
        [Required]
        public decimal Q1FYLENaira { get; set; }

        [Display(Name = " Q1 FYLE(S'USD)" )]
        [Required]
        public decimal Q1FYLEDollar { get; set; }

        [Display(Name = " Q1 FYLE(F'USD)" )]
        [Required]
        public decimal Q1FYLEFDollar { get; set; }

        [Display(Name = " Q2 FYLE(S'NGN)" )]
        [Required]
        public decimal Q2FYLENaira { get; set; }

        [Display(Name = " Q2 FYLE(S'USD)" )]
        [Required]
        public decimal Q2FYLEDollar { get; set; }

        [Display(Name = " Q2 FYLE(F'USD)" )]
        [Required]
        public decimal Q2FYLEFDollar { get; set; }

        [Display(Name = " Q3 FYLE(S'NGN)" )]
        [Required]
        public decimal Q3FYLENaira { get; set; }

        [Display(Name = " Q3 FYLE(S'USD)" )]
        [Required]
        public decimal Q3FYLEDollar { get; set; }

        [Display(Name = " Q3 FYLE(F'USD)" )]
        [Required]
        public decimal Q3FYLEFDollar { get; set; }

        [Display(Name = " Q4 FYLE(S'NGN)" )]
        [Required]
        public decimal Q4FYLENaira { get; set; }

        [Display(Name = " Q4 FYLE(S'USD)" )]
        [Required]
        public decimal Q4FYLEDollar { get; set; }

        [Display(Name = " Q4 FYLE(F'USD)" )]
        [Required]
        public decimal Q4FYLEFDollar { get; set; }

        [Display(Name = "Year")]
        public int YYear { get; set; }

        [Display(Name = "Line Manager")]
        public string ActivityCodeLineManager { get; set; }

        [Display(Name = " Commitment (S'USD)")]
        public decimal Commitments { get; set; }

    }
}