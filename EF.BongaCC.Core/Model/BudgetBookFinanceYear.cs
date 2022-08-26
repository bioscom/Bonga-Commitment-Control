using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class BudgetBookFinanceYear : BaseEntity
    {
        public long? BudgetBookID { get; set; }
        public virtual BudgetBook BudgetBook { get; set; }
        
        public decimal OPYearBudgetNaira { get; set; }
        public decimal OPYearBudgetDollar { get; set; }
        public decimal OPYearBudgetFDollar { get; set; }
        public decimal NAPIMSBUDGETNaira { get; set; }
        public decimal NAPIMSBUDGETDollar { get; set; }
        public decimal NAPIMSBUDGETFDollar { get; set; }

        public decimal Q1FYLENaira { get; set; }
        public decimal Q1FYLEDollar { get; set; }
        public decimal Q1FYLEFDollar { get; set; }

        public decimal Q2FYLENaira { get; set; }
        public decimal Q2FYLEDollar { get; set; }
        public decimal Q2FYLEFDollar { get; set; }

        public decimal Q3FYLENaira { get; set; }
        public decimal Q3FYLEDollar { get; set; }
        public decimal Q3FYLEFDollar { get; set; }

        public decimal Q4FYLENaira { get; set; }
        public decimal Q4FYLEDollar { get; set; }
        public decimal Q4FYLEFDollar { get; set; }

        public int YYear { get; set; }
    }
}
