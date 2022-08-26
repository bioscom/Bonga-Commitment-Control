using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

/// <summary>
/// Summary description for ActivityDetails
/// </summary>
/// 
namespace EF.BongaCC.Core.Model
{
    public class ActivityDetails : BaseEntity
    {
        public string Description { get; set; }
        //public decimal m_dAmount { get; set; } calculated
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public int iYear { get; set; }

        public decimal? FixedExchangeRate { get; set; }

        public long? BudgetBookCommitmentsID { get; set; }
        public virtual BudgetBookCommitments BudgetBookCommitments { get; set; }

        public long? BudgetBookID { get; set; }
        public virtual BudgetBook BudgetBook { get; set; }

        public long? CurrenciesID { get; set; }
        public virtual Currencies Currencies { get; set; }
    }
}