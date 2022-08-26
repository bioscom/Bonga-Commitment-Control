using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class BudgetBook : BaseEntity
    {
        //public int CapexOpex { get; set; }
        public int DirectAllocated { get; set; }
        public long UapCodeID { get; set; }
        public virtual UapCode UapCode { get; set; }
        public long UapRollUpCodeID { get; set; }
        public virtual UapRollUpCode UapRollUpCode { get; set; }
        public long ActivityTypeID { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public long ActivityCodeID { get; set; }
        public virtual ActivityCode ActivityCode { get; set; }
        public long wbsID { get; set; }
        public virtual WBS WBS { get; set; }
        public long ActivityID { get; set; }
        public virtual Activity Activity { get; set; }
        //public long? ActivityOwnerID { get; set; }
        //public long? LineManagerID { get; set; }
        //public long? SponsorID { get; set; }
        //public virtual AppUsers Sponsor { get; set; }
        public long ScopeID { get; set; }
        public virtual Scope Scope { get; set; }
        public long ContractID { get; set; }
        public virtual Contract Contract { get; set; }
        public long BudgetBasisID { get; set; }
        public virtual BudgetBasis BudgetBasis { get; set; }

        public long CapexOpexID { get; set; }
        public virtual CXOX CapexOpex { get; set; }

        public decimal NAPIMSBUDGETNaira { get; set; }
        public decimal NAPIMSBUDGETDollar { get; set; }
        public decimal NAPIMSBUDGETFDollar { get; set; }

        public int YYear { get; set; }

        ////
        //public decimal OPYearBudgetNaira { get; set; }
        //public decimal OPYearBudgetDollar { get; set; }
        //public decimal OPYearBudgetFDollar { get; set; }
        //public decimal NAPIMSBUDGETNaira { get; set; }
        //public decimal NAPIMSBUDGETDollar { get; set; }
        //public decimal NAPIMSBUDGETFDollar { get; set; }
        //public decimal Q1FYLENaira { get; set; }
        //public decimal Q1FYLEDollar { get; set; }
        //public decimal Q1FYLEFDollar { get; set; }

        //public decimal Q2FYLENaira { get; set; }
        //public decimal Q2FYLEDollar { get; set; }
        //public decimal Q2FYLEFDollar { get; set; }
        //public decimal Q3FYLENaira { get; set; }
        //public decimal Q3FYLEDollar { get; set; }
        //public decimal Q3FYLEFDollar { get; set; }
        //public decimal Q4FYLENaira { get; set; }
        //public decimal Q4FYLEDollar { get; set; }
        //public decimal Q4FYLEFDollar { get; set; }

        public virtual ICollection<BudgetBookCommitments> BudgetBookCommitments { get; set; }
        public virtual ICollection<ActivityDetails> ActivityDetails { get; set; }
        //public virtual ICollection<BudgetBookFinanceYear> BudgetBookFinanceYears { get; set; }
    }
}
