using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Web.BongaCC.ViewModels
{
    public class BudgetBookCommitmentViewModel
    {
        [ScaffoldColumn(false)]
        public long? ID { get; set; }

        public long? BudgetBookID { get; set; }

        [Display(Name = "Capex / Opex")]
        [Required]
        public long CapexOpexID { get; set; }

        [Display(Name = "Capex / Opex")]
        public string CapexOpex { get; set; }


        [Display(Name = "PR Number")]
        [Required]
        public string PRNumber { get; set; }

        [Display(Name = "PR Value (F$)")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal PRValue { get; set; }

        [Display(Name = "PO Number")]
        [Required]
        public string PONumber { get; set; }

        [Display(Name = "PO Value (F$)")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal POValue { get; set; }


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

        [Display(Name = " PLAN(F'USD)")]
        [Required]
        public decimal OPYearBudgetFDollar { get; set; }

        [Display(Name = " NAPIMS BUDGET(F'USD)")]
        [Required]
        public decimal NAPIMSBUDGETFDollar { get; set; }

        [Display(Name = " Q1 FYLE(F'USD)")]
        [Required]
        public decimal Q1FYLEFDollar { get; set; }

        [Display(Name = "BCC No.")]
        public string Comitmntno { get; set; }

        [Display(Name = "Commitment(F$)")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Commitment { get; set; }

        //Date of Target CCP session
        [Display(Name = "CCP Session Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        [Required]
        public DateTime? CCPSessionDate { get; set; }

        [Display(Name = "Focal Point")]
        [Required]
        public long? FocalPointID { get; set; }

        [Display(Name = "Focal Point")]
        public string FocalPoint { get; set; }

        ////approval process

        [Display(Name = "Stand:")]
        public long? ApprovalID { get; set; }

        [Display(Name = "Status")]
        public string sApprovalID { get; set; }

        public string ApprovalStatus { get; set; }


        [Display(Name = "Approval Comment")]
        [Required]
        public string ApprovalComment { get; set; }

        [Display(Name = "Savings")]
        public decimal? Savings { get; set; }

        [Display(Name = "Reviewed/Approved By")]
        public long? ApproverID { get; set; }

        [Display(Name = "Approved By")]
        public string Approver { get; set; }

        [Display(Name = "Activity Description")]
        [Required]
        public string title { get; set; }
        public int iYear { get; set; }

        [Display(Name = "Line Manager")]

        public long? LineManagerID { get; set; }

        [Display(Name = "Line Manager")]
        public string LineManagerFullName { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [UIHint("Date")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Week Submitted")]
        public int? AddedDateWeek { get; set; }

        //New additions

        [Display(Name = "Business Justification")]
        [Required]
        public string justification { get; set; }

        [Display(Name = "Regrets / Implication of non approval")]
        [Required]
        public string threat { get; set; }

        [Display(Name = "Contract No., Vendor & CP Focal Point")]
        public string contractnovendor { get; set; }

        [Display(Name = "Activity Period")]
        public string sPeriodfrom { get; set; }

        [Display(Name = "Purchasing Group")]
        [Required]
        public long? groupID { get; set; }
        public string Group { get; set; }
        [Display(Name = "Team")]
        public long? teamID { get; set; }
        public string sTeam { get; set; }

        [Display(Name = "Planned or Emmergency")]
        [Required]
        public long? typeID { get; set; }
        public string PlannedEmmergency { get; set; }

        [Display(Name = "Request Status")]
        [Required]
        public long? statusID { get; set; }
        public string Status { get; set; }

        [Display(Name = "Contract/Proc. Vehicle")]
        [Required]
        public long? vehicleID { get; set; }

        public string VehicleProcurement { get; set; }

        public int WorkFlowType { get; set; }

        //[Display(Name = " BUDGET(S'NGN)" )]
        //[Required]
        //public decimal OPYearBudgetNaira { get; set; }

        //[Display(Name = " BUDGET(S'USD)" )]
        //[Required]
        //public decimal OPYearBudgetDollar { get; set; }

        //[Display(Name = " NAPIMS BUDGET(S'NGN)" )]
        //[Required]
        //public decimal NAPIMSBUDGETNaira { get; set; }

        //[Display(Name = " NAPIMS BUDGET(S'USD)" )]
        //[Required]
        //public decimal NAPIMSBUDGETDollar { get; set; }

        //[Display(Name = " Q1 FYLE(S'NGN)" )]
        //[Required]
        //public decimal Q1FYLENaira { get; set; }

        //[Display(Name = " Q1 FYLE(S'USD)" )]
        //[Required]
        //public decimal Q1FYLEDollar { get; set; }

        //[Display(Name = " Q2 FYLE(S'NGN)" )]
        //[Required]
        //public decimal Q2FYLENaira { get; set; }

        //[Display(Name = " Q2 FYLE(S'USD)" )]
        //[Required]
        //public decimal Q2FYLEDollar { get; set; }

        //[Display(Name = " Q2 FYLE(F'USD)" )]
        //[Required]
        //public decimal Q2FYLEFDollar { get; set; }

        //[Display(Name = " Q3 FYLE(S'NGN)" )]
        //[Required]
        //public decimal Q3FYLENaira { get; set; }

        //[Display(Name = " Q3 FYLE(S'USD)" )]
        //[Required]
        //public decimal Q3FYLEDollar { get; set; }

        //[Display(Name = " Q3 FYLE(F'USD)" )]
        //[Required]
        //public decimal Q3FYLEFDollar { get; set; }

        //[Display(Name = " Q4 FYLE(S'NGN)" )]
        //[Required]
        //public decimal Q4FYLENaira { get; set; }

        //[Display(Name = " Q4 FYLE(S'USD)" )]
        //[Required]
        //public decimal Q4FYLEDollar { get; set; }

        //[Display(Name = " Q4 FYLE(F'USD)" )]
        //[Required]
        //public decimal Q4FYLEFDollar { get; set; }

        //[Display(Name = "Year")]
        //public int YYear { get; set; }

        //[Display(Name = "Capex / Opex")]
        //[EnumDataType(typeof(enuCapexOpex))]
        //[Required]
        //public long CapexOpexID { get; set; }

        //[Display(Name = "Capex / Opex")]
        //public string CapexOpex { get; set; }

        //[Display(Name = "Direct / Allocated")]
        //[EnumDataType(typeof(enuDirectAllocated))]
        //[Required]
        //public int DirectAllocated { get; set; }

        //[Display(Name = "Direct / Allocated")]
        //public string sDirectAllocated { get; set; }

        //[Display(Name = "UAP Code")]
        //[Required]
        //public long UapCodeID { get; set; }

        //[Display(Name = "UAP Code")]
        //public string UapCode { get; set; }

        //[Display(Name = "UAP Roll Up Code")]
        //[Required]
        //public long UapRollUpCodeID { get; set; }

        //[Display(Name = "UAP Roll Up Code")]
        //public string UapRollUpCode { get; set; }

        //[Display(Name = "Activity Name")]
        //[Required]
        //public long ActivityTypeID { get; set; }

        //[Display(Name = "Activity Name")]
        //public string ActivityType { get; set; }

        //[Display(Name = "Activity Code")]
        //[Required]
        //public long ActivityCodeID { get; set; }

        //[Display(Name = "Activity Code")]
        //public string ActivityCode { get; set; }

        //[Display(Name = "Cost Object")]
        //[Required]
        //public long wbsID { get; set; }

        //[Display(Name = "Cost Object")]
        //public string CostObject { get; set; }

        //[Display(Name = "Activity")]
        //[Required]
        //public long ActivityID { get; set; }

        //[Display(Name = "Activity")]
        //public string Activity { get; set; }



        //[Display(Name = "Accountable Manager")]
        //[Required]
        //public long? SponsorID { get; set; }

        //[Display(Name = "Accountable Manager")]
        //public string Sponsor { get; set; }

        //[Display(Name = "Scope")]
        //[Required]
        //public long ScopeID { get; set; }

        //[Display(Name = "Scope")]
        //public string Scope { get; set; }

        //[Display(Name = "Contract")]
        //[Required]
        //public long ContractID { get; set; }

        //[Display(Name = "Contract")]
        //public string Contract { get; set; }

        //[Display(Name = "Budget Basis")]
        //[Required]
        //public long BudgetBasisID { get; set; }

        //[Display(Name = "Budget Basis")]
        //public string BudgetBasis { get; set; }

        //[Display(Name = " PLAN(F'USD)" )]
        //[Required]
        //public decimal OPYearBudgetFDollar { get; set; }

        //[Display(Name = " NAPIMS BUDGET(F'USD)" )]
        //[Required]
        //public decimal NAPIMSBUDGETFDollar { get; set; }

        //[Display(Name = " Q1 FYLE(F'USD)" )]
        //[Required]
        //public decimal Q1FYLEFDollar { get; set; }

    }
}