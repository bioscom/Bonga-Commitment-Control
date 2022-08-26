using EF.BongaCC.Core.Model;
using System.Collections.Generic;

namespace Web.BongaCC.ViewModels
{
    public class CommitmentControlViewModel
    {
        public ActivityDetailsViewModel CostBreakdown { get; set; }
        public IEnumerable<ActivityDetailsViewModel> lstCostBreakdown { get; set; }
        public IEnumerable<ActivityDetailsViewModel> lstTotalCostBreakdown { get; set; }
        public ExchangeRate ExchangeRate { get; set; }
        public UploadFilesViewModel FileUpload { get; set; }
        public IEnumerable<UploadFilesViewModel> LstUploadFiles { get; set; }
        public BudgetBookViewModel oBudgetBook { get; set; }
        public BudgetBookCommitmentViewModel oBudgetBookCommitment { get; set; }
        public List<CXOX> CapexOpexList { get; set; }
        public List<Currencies> lstCurrencies { get; set; }


        public IEnumerable<CommitmentsViewModel> lstCommitments { get; set; }
        public CommitmentsViewModel oCommitment { get; set; }
        public CommitmentsViewModel oCommitmentCompare { get; set; }
        public ActivityDetailsViewModel CostBreakdownCompare { get; set; }
        public IEnumerable<ActivityDetailsViewModel> lstCostBreakdownCompare { get; set; }
        public IEnumerable<UploadFilesViewModel> LstUploadFilesCompare { get; set; }
        public IEnumerable<BudgetBookViewModel> lstBudgetBooks { get; set; }
        public IEnumerable<BudgetBookCommitmentViewModel> lstBudgetBooksCommitment { get; set; }
    }
}
