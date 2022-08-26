using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.ViewModels
{
    public class AppReportingViewModel
    {
        public decimal? TotalCapexPending { get; set; }
        public decimal? TotalCapexApproved { get; set; }
        public decimal? TotalCapexRejected { get; set; }
        public int TotalNoCapexPending { get; set; }
        public int TotalNoCapexApproved { get; set; }
        public int TotalNoCapexRejected { get; set; }
        public decimal? TotalCapexCommitment { get; set; }
        public int TotalNoOfCapexCommitments { get; set; }
        public decimal? TotalCapexSavings { get; set; }


        public decimal? TotalOpexPending { get; set; }
        public decimal? TotalOpexApproved { get; set; }
        public decimal? TotalOpexRejected { get; set; }
        public int TotalNoOpexPending { get; set; }
        public int TotalNoOpexApproved { get; set; }
        public int TotalNoOpexRejected { get; set; }
        public decimal? TotalOpexCommitment { get; set; }
        public int TotalNoOfOpexCommitments { get; set; }
        public decimal? TotalOpexSavings { get; set; }


        public decimal? TotalPending { get; set; }
        public decimal? TotalApproved { get; set; }
        public decimal? TotalRejected { get; set; }
        public int TotalNoPending { get; set; }
        public int TotalNoApproved { get; set; }
        public int TotalNoRejected { get; set; }

        public decimal? TotalCommitment { get; set; }

        public int TotalNoOfCommitments { get; set; }

        public decimal? TotalSavings { get; set; }
    }
}
