using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF.BongaCC.Core.Model;
using EF.BongaCC.Data;
using Web.BongaCC.ViewModels;
using EF.BongaCC.Data.Repository;
using Microsoft.AspNetCore.Http;
using Web.BongaCC.Codes;

namespace Web.BongaCC.Controllers
{
    public class BudgetBooksController : Controller
    {
        private readonly IRepository<BudgetBook> repo;
        private readonly IRepository<BudgetBookCommitments> repoBudgetBookCommitments;
        private readonly IRepository<BudgetBasis> repoBudgetBasis;
        private readonly IRepository<Activity> repoActivity;
        private readonly IRepository<ActivityType> repoActivityName;
        private readonly IRepository<Scope> repoScope;
        private readonly IRepository<Contract> repoContract;
        private readonly IRepository<UapCode> repoUapCode;
        private readonly IRepository<UapRollUpCode> repoUapRollUpCode;
        private readonly IRepository<ActivityCode> repoActivityCode;
        private readonly IRepository<WBS> repoWBS;
        private readonly IRepository<AppUsers> repoUsers;
        private readonly IRepository<CXOX> repoCapexOpex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IRepository<ActivityDetails> repoCostBreakdown;
        private readonly IRepository<ExchangeRate> repoExchRate;
        private readonly IRepository<Currencies> repoCurrencies;
        //private readonly CommitmentsController CommitController;

        public BudgetBooksController(IRepository<BudgetBook> repo, IRepository<BudgetBasis> repoBudgetBasis, IRepository<Activity> repoActivity, IRepository<ActivityDetails> repoCostBreakdown,
        IRepository<ActivityType> repoActivityName, IRepository<Scope> repoScope, IRepository<Contract> repoContract, IRepository<BudgetBookCommitments> repoBudgetBookCommitments,
        IRepository<UapCode> repoUapCode, IRepository<UapRollUpCode> repoUapRollUpCode, IRepository<ActivityCode> repoActivityCode, IRepository<ExchangeRate> repoExchRate, IRepository<Currencies> repoCurrencies,
        IRepository<WBS> repoWBS, IRepository<AppUsers> repoUsers, IRepository<CXOX> repoCapexOpex, IHttpContextAccessor httpContextAccessor)
        {
            this.repo = repo; this.repoBudgetBasis = repoBudgetBasis; this.repoActivity = repoActivity; this.repoCostBreakdown = repoCostBreakdown;
            this.repoActivityName = repoActivityName; this.repoScope = repoScope; this.repoContract = repoContract;
            this.repoUapCode = repoUapCode; this.repoUapRollUpCode = repoUapRollUpCode; this.repoActivityCode = repoActivityCode;
            this.repoWBS = repoWBS; this.repoUsers = repoUsers; this.repoCapexOpex = repoCapexOpex; _httpContextAccessor = httpContextAccessor;
            this.repoBudgetBookCommitments = repoBudgetBookCommitments; this.repoExchRate = repoExchRate; this.repoCurrencies = repoCurrencies;
        }

        // GET: BudgetBooks
        public IActionResult Index()
        {
            ViewBag.DirectAllocated = new SelectList(RolesManager.GetDirectAllocated(), "Value", "Text");
            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex");
            ViewBag.ActivityName = new SelectList(repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName).ToList(), "ID", "ActivityName");

            ViewBag.UapCodeID = new SelectList((repoUapCode.GetAll() != null) ? repoUapCode.GetAll().Result.OrderBy(o => o.UapCodeDesc).ToList() : null, "ID", "UapCodeDesc");
            ViewBag.UapRollUpCodeID = new SelectList((repoUapRollUpCode.GetAll() != null) ? repoUapRollUpCode.GetAll().Result.OrderBy(o => o.UapRollUpCodeDesc).ToList() : null, "ID", "UapRollUpCodeDesc");

            ViewBag.Activity = new SelectList((repoActivity.GetAll() != null) ? repoActivity.GetAll().Result.OrderBy(o => o.Description).ToList() : null, "ID", "Description");
            ViewBag.ActivityCode = new SelectList((repoActivityCode.GetAll() != null) ? repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList() : null, "ID", "ActivityCodeDesc");
            ViewBag.BudgetBasis = new SelectList((repoBudgetBasis.GetAll() != null) ? repoBudgetBasis.GetAll().Result.OrderBy(o => o.BudgetBase).ToList() : null, "ID", "BudgetBase");
            ViewBag.Contract = new SelectList((repoContract.GetAll() != null) ? repoContract.GetAll().Result.OrderBy(o => o.ContractName).ToList() : null, "ID", "ContractName");
            ViewBag.Scopes = new SelectList((repoScope.GetAll() != null) ? repoScope.GetAll().Result.OrderBy(o => o.Purpose).ToList() : null, "ID", "Purpose");
            //ViewBag.Sponsors = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList() : null, "ID", "FullName");
            //ViewBag.ActivityOwners = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList() : null, "ID", "FullName");
            //ViewBag.LineManagers = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.LineManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.LineManager).ToList() : null, "ID", "FullName");
            ViewBag.WBS = new SelectList((repoWBS.GetAll() != null) ? repoWBS.GetAll().Result.OrderBy(o => o.CostObjectsDescription).ToList() : null, "ID", "CostObjects");

            var CapexOpex = repo.GetAll().Result.Where(t => t.YYear == DateTime.Today.Year);
            var Capex = CapexOpex.Where(o => o.CapexOpex.CapexOpex.Contains("Capex")).Sum(t => t.NAPIMSBUDGETFDollar);
            var Opex = CapexOpex.Where(o => o.CapexOpex.CapexOpex.Contains("Opex")).Sum(t => t.NAPIMSBUDGETFDollar);
            
            var CapexExp = GetTotalSumBudgetBookCostBreakDownByCapex();
            var OpexExp = GetTotalSumBudgetBookCostBreakDownByOpex();
            
            var CapexBal = Capex - CapexExp;
            var OpexBal = Opex - OpexExp;

            ViewBag.Capex = stringRoutine.formatAsBankMoney("$", Capex);
            ViewBag.Opex = stringRoutine.formatAsBankMoney("$", Opex);
            ViewBag.CapexExp = stringRoutine.formatAsBankMoney("$", CapexExp);
            ViewBag.OpexExp = stringRoutine.formatAsBankMoney("$", OpexExp);
            ViewBag.CapexBal = stringRoutine.formatAsBankMoney("$", CapexBal);
            ViewBag.OpexBal = stringRoutine.formatAsBankMoney("$", OpexBal);

            return View();
        }

        public IActionResult AOFilter()
        {
            ViewBag.DirectAllocated = new SelectList(RolesManager.GetDirectAllocated(), "Value", "Text");
            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex");
            ViewBag.ActivityName = new SelectList(repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName).ToList(), "ID", "ActivityName");

            ViewBag.UapCodeID = new SelectList((repoUapCode.GetAll() != null) ? repoUapCode.GetAll().Result.OrderBy(o => o.UapCodeDesc).ToList() : null, "ID", "UapCodeDesc");
            ViewBag.UapRollUpCodeID = new SelectList((repoUapRollUpCode.GetAll() != null) ? repoUapRollUpCode.GetAll().Result.OrderBy(o => o.UapRollUpCodeDesc).ToList() : null, "ID", "UapRollUpCodeDesc");

            ViewBag.Activity = new SelectList((repoActivity.GetAll() != null) ? repoActivity.GetAll().Result.OrderBy(o => o.Description).ToList() : null, "ID", "Description");
            ViewBag.ActivityCode = new SelectList((repoActivityCode.GetAll() != null) ? repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList() : null, "ID", "ActivityCodeDesc");
            ViewBag.BudgetBasis = new SelectList((repoBudgetBasis.GetAll() != null) ? repoBudgetBasis.GetAll().Result.OrderBy(o => o.BudgetBase).ToList() : null, "ID", "BudgetBase");
            ViewBag.Contract = new SelectList((repoContract.GetAll() != null) ? repoContract.GetAll().Result.OrderBy(o => o.ContractName).ToList() : null, "ID", "ContractName");
            ViewBag.Scopes = new SelectList((repoScope.GetAll() != null) ? repoScope.GetAll().Result.OrderBy(o => o.Purpose).ToList() : null, "ID", "Purpose");
            ViewBag.WBS = new SelectList((repoWBS.GetAll() != null) ? repoWBS.GetAll().Result.OrderBy(o => o.CostObjectsDescription).ToList() : null, "ID", "CostObjects");

            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");

            return View();
        }

        private IEnumerable<BudgetBookViewModel> GetBudgetBook(long? id)
        {
            var BudgetBases = repoBudgetBasis.GetAll().Result.ToList();
            var Activities = repoActivity.GetAll().Result.ToList();
            var ActivityNames = repoActivityName.GetAll().Result.ToList();
            var Scopes = repoScope.GetAll().Result.ToList();
            var Contracts = repoContract.GetAll().Result.ToList();
            var UapCodes = repoUapCode.GetAll().Result.ToList();
            var UapRollUpCodes = repoUapRollUpCode.GetAll().Result.ToList();
            var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
            var WBSes = repoWBS.GetAll().Result.ToList();
            var CapexOpexs = repoCapexOpex.GetAll().Result.ToList();
            var BudgetBooke = repo.GetAll().Result.ToList();

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            var result = repo.GetAll().Result.Where(c => c.ID == id).ToList().Select(entity =>
            {
                var BudgetBook = new BudgetBook();
                var BudgetBasis = new BudgetBasis();
                var Activity = new Activity();
                var ActivityName = new ActivityType();
                var Scope = new Scope();
                var Contract = new Contract();
                var UapCode = new UapCode();
                var Uaprollupcode = new UapRollUpCode();
                var ActivityCode = new ActivityCode();
                var wbs = new WBS();
                var CapexOpex = new CXOX();

                if (BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID);
                if (Activities.FirstOrDefault(c => entity.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => entity.ActivityID == c.ID);
                if (ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID);
                //var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
                if (Scopes.FirstOrDefault(c => entity.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => entity.ScopeID == c.ID);
                if (Contracts.FirstOrDefault(c => entity.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => entity.ContractID == c.ID);
                if (UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID);
                if (UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID);
                if (ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID);
                if (WBSes.FirstOrDefault(c => entity.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => entity.wbsID == c.ID);
                if (CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID);

                return new BudgetBookViewModel
                {
                    ID = entity.ID,
                    CapexOpexID = entity.CapexOpexID,
                    CapexOpex = CapexOpex.CapexOpex,
                    DirectAllocated = entity.DirectAllocated,
                    sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(entity.DirectAllocated))).ToString(),
                    UapCodeID = entity.UapCodeID,
                    UapCode = UapCode.UapCodeDesc,
                    UapRollUpCodeID = entity.UapRollUpCodeID,
                    UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                    ActivityTypeID = entity.ActivityTypeID,
                    ActivityType = ActivityName.ActivityName,
                    ActivityCode = ActivityCode.ActivityCodeDesc,
                    wbsID = entity.wbsID,
                    CostObject = wbs.CostObjects,
                    ActivityID = entity.ActivityID,
                    Activity = Activity.Description,

                    ActivityCodeID = entity.ActivityCodeID,
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    ActivityOwnerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().ID,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    LineManagerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().ID,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,
                    SponsorID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().ID,

                    ScopeID = entity.ScopeID,
                    Scope = Scope.Purpose,
                    ContractID = entity.ContractID,
                    Contract = Contract.ContractName,
                    BudgetBasisID = entity.BudgetBasisID,
                    BudgetBasis = BudgetBasis.BudgetBase,

                    //OPYearBudgetNaira = entity.OPYearBudgetNaira,
                    //OPYearBudgetDollar = entity.OPYearBudgetDollar,
                    //OPYearBudgetFDollar = entity.OPYearBudgetFDollar,
                    //NAPIMSBUDGETNaira = entity.NAPIMSBUDGETNaira,
                    //NAPIMSBUDGETDollar = entity.NAPIMSBUDGETDollar,
                    NAPIMSBUDGETFDollar = entity.NAPIMSBUDGETFDollar,
                    YYear = entity.YYear,
                };
            }).ToList();
            return result;
        }

        private IEnumerable<BudgetBookViewModel> GetBudgetBooks(int? iYear, long? IdCapexOpex, long? IdActivityOwner, long? IdActivityCode, long? IdActivityName, long? IdScope, long? IdBudgetBasis)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            var BudgetBases = repoBudgetBasis.GetAll().Result.ToList();
            var Activities = repoActivity.GetAll().Result.ToList();
            var ActivityNames = repoActivityName.GetAll().Result.ToList();
            var Scopes = repoScope.GetAll().Result.ToList();
            var Contracts = repoContract.GetAll().Result.ToList();
            var UapCodes = repoUapCode.GetAll().Result.ToList();
            var UapRollUpCodes = repoUapRollUpCode.GetAll().Result.ToList();
            var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
            var WBSes = repoWBS.GetAll().Result.ToList();
            var CapexOpexs = repoCapexOpex.GetAll().Result.ToList();

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            IEnumerable<BudgetBook> query = null;

            if (IdActivityOwner == 0 && IdActivityCode == 0 && IdActivityName == 0 && IdScope == 0 && IdBudgetBasis == 0)
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex);
            else if(IdActivityCode == 0 && IdActivityName == 0 && IdScope == 0 && IdBudgetBasis == 0)
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex && o.ActivityCode.ActivityOwnerID == IdActivityOwner);
            else if (IdActivityName == 0 && IdScope == 0 && IdBudgetBasis == 0)
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex && o.ActivityCode.ActivityOwnerID == IdActivityOwner && o.ActivityCodeID == IdActivityCode);
            else if (IdScope == 0 && IdBudgetBasis == 0)
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex && o.ActivityCode.ActivityOwnerID == IdActivityOwner && o.ActivityCodeID == IdActivityCode && o.ActivityTypeID == IdActivityName);
            else if (IdBudgetBasis == 0)
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex && o.ActivityCode.ActivityOwnerID == IdActivityOwner && o.ActivityCodeID == IdActivityCode && o.ActivityTypeID == IdActivityName && o.ScopeID == IdScope);
            else
                query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.CapexOpexID == IdCapexOpex && o.ActivityCode.ActivityOwnerID == IdActivityOwner && o.ActivityCodeID == IdActivityCode && o.ActivityTypeID == IdActivityName && o.ScopeID == IdScope && o.BudgetBasisID == IdBudgetBasis);

            var result = query.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                var BudgetBook = new BudgetBook();
                var BudgetBasis = new BudgetBasis();
                var Activity = new Activity();
                var ActivityName = new ActivityType();
                var Scope = new Scope();
                var Contract = new Contract();
                var UapCode = new UapCode();
                var Uaprollupcode = new UapRollUpCode();
                var ActivityCode = new ActivityCode();
                var wbs = new WBS();
                var CapexOpex = new CXOX();

                if (BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID);
                if (Activities.FirstOrDefault(c => entity.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => entity.ActivityID == c.ID);
                if (ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID);
                //var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
                if (Scopes.FirstOrDefault(c => entity.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => entity.ScopeID == c.ID);
                if (Contracts.FirstOrDefault(c => entity.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => entity.ContractID == c.ID);
                if (UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID);
                if (UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID);
                if (ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID);
                if (WBSes.FirstOrDefault(c => entity.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => entity.wbsID == c.ID);
                if (CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID);

                return new BudgetBookViewModel
                {
                    ID = entity.ID,
                    CapexOpexID = entity.CapexOpexID,
                    CapexOpex = CapexOpex.CapexOpex,
                    DirectAllocated = entity.DirectAllocated,
                    sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(entity.DirectAllocated))).ToString(),
                    UapCodeID = entity.UapCodeID,
                    UapCode = UapCode.UapCodeDesc,
                    UapRollUpCodeID = entity.UapRollUpCodeID,
                    UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                    ActivityTypeID = entity.ActivityTypeID,
                    ActivityType = ActivityName.ActivityName,
                    ActivityCode = ActivityCode.ActivityCodeDesc,
                    wbsID = entity.wbsID,
                    CostObject = wbs.CostObjects,
                    ActivityID = entity.ActivityID,
                    Activity = Activity.Description,

                    ActivityCodeID = entity.ActivityCodeID,
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,

                    ActivityCodeLineManager = ActivityCode.LineManager.FullName,
                    ScopeID = entity.ScopeID,
                    Scope = Scope.Purpose,
                    ContractID = entity.ContractID,
                    Contract = Contract.ContractName,
                    BudgetBasisID = entity.BudgetBasisID,
                    BudgetBasis = BudgetBasis.BudgetBase,

                    //OPYearBudgetNaira = entity.OPYearBudgetNaira,
                    //OPYearBudgetDollar = entity.OPYearBudgetDollar,
                    //OPYearBudgetFDollar = entity.OPYearBudgetFDollar,
                    //NAPIMSBUDGETNaira = entity.NAPIMSBUDGETNaira,
                    //NAPIMSBUDGETDollar = entity.NAPIMSBUDGETDollar,
                    NAPIMSBUDGETFDollar = entity.NAPIMSBUDGETFDollar,
                    //YYear = BBookFinanceYear.YYear,
                    //Q1FYLENaira = BBookFinanceYear.Q1FYLENaira,
                    //Q1FYLEDollar = BBookFinanceYear.Q1FYLEDollar,
                    //Q1FYLEFDollar = BBookFinanceYear.Q1FYLEFDollar,

                    //Q2FYLENaira = BBookFinanceYear.Q2FYLENaira,
                    //Q2FYLEDollar = BBookFinanceYear.Q2FYLEDollar,
                    //Q2FYLEFDollar = BBookFinanceYear.Q2FYLEFDollar,

                    //Q3FYLENaira = BBookFinanceYear.Q3FYLENaira,
                    //Q3FYLEDollar = BBookFinanceYear.Q3FYLEDollar,
                    //Q3FYLEFDollar = BBookFinanceYear.Q3FYLEFDollar,

                    //Q4FYLENaira = BBookFinanceYear.Q4FYLENaira,
                    //Q4FYLEDollar = BBookFinanceYear.Q4FYLEDollar,
                    //Q4FYLEFDollar = BBookFinanceYear.Q4FYLEFDollar,
                };
            }).ToList();
            return result;
        }

        private IEnumerable<BudgetBookViewModel> GetBudgetBooks2(int? iYear, long? IdActivityOwner, long? IdActivityCode)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            var BudgetBases = repoBudgetBasis.GetAll().Result.ToList();
            var Activities = repoActivity.GetAll().Result.ToList();
            var ActivityNames = repoActivityName.GetAll().Result.ToList();
            var Scopes = repoScope.GetAll().Result.ToList();
            var Contracts = repoContract.GetAll().Result.ToList();
            var UapCodes = repoUapCode.GetAll().Result.ToList();
            var UapRollUpCodes = repoUapRollUpCode.GetAll().Result.ToList();
            var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
            var WBSes = repoWBS.GetAll().Result.ToList();
            var CapexOpexs = repoCapexOpex.GetAll().Result.ToList();
            var Commitments = repoBudgetBookCommitments.GetAll().Result.ToList();

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            IEnumerable<BudgetBook> query = null;

            if (IdActivityOwner == 0 && IdActivityCode == 0) query = repo.GetAll().Result.Where(o => o.YYear == iYyear);
            else if (IdActivityCode == 0) query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.ActivityCode.ActivityOwnerID == IdActivityOwner);            
            else query = repo.GetAll().Result.Where(o => o.YYear == iYyear && o.ActivityCode.ActivityOwnerID == IdActivityOwner && o.ActivityCodeID == IdActivityCode);

            var result = query.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                var BudgetBook = new BudgetBook();
                var BudgetBasis = new BudgetBasis();
                var Activity = new Activity();
                var ActivityName = new ActivityType();
                var Scope = new Scope();
                var Contract = new Contract();
                var UapCode = new UapCode();
                var Uaprollupcode = new UapRollUpCode();
                var ActivityCode = new ActivityCode();
                var wbs = new WBS();
                var CapexOpex = new CXOX();
                var Commitment = new BudgetBookCommitments();

                if (BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID);
                if (Activities.FirstOrDefault(c => entity.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => entity.ActivityID == c.ID);
                if (ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID);
                //var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
                if (Scopes.FirstOrDefault(c => entity.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => entity.ScopeID == c.ID);
                if (Contracts.FirstOrDefault(c => entity.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => entity.ContractID == c.ID);
                if (UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID);
                if (UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID);
                if (ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID);
                if (WBSes.FirstOrDefault(c => entity.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => entity.wbsID == c.ID);
                if (CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID);
                if (Commitments.FirstOrDefault(c => entity.ID == c.BudgetBookID) != null) Commitment = Commitments.FirstOrDefault(c => entity.ID == c.BudgetBookID);

                return new BudgetBookViewModel
                {
                    ID = entity.ID,
                    CapexOpexID = entity.CapexOpexID,
                    CapexOpex = CapexOpex.CapexOpex,
                    DirectAllocated = entity.DirectAllocated,
                    sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(entity.DirectAllocated))).ToString(),
                    UapCodeID = entity.UapCodeID,
                    UapCode = UapCode.UapCodeDesc,
                    UapRollUpCodeID = entity.UapRollUpCodeID,
                    UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                    ActivityTypeID = entity.ActivityTypeID,
                    ActivityType = ActivityName.ActivityName,
                    ActivityCode = ActivityCode.ActivityCodeDesc,
                    wbsID = entity.wbsID,
                    CostObject = wbs.CostObjects,
                    ActivityID = entity.ActivityID,
                    Activity = Activity.Description,

                    ActivityCodeID = entity.ActivityCodeID,
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,

                    ActivityCodeLineManager = ActivityCode.LineManager.FullName,
                    ScopeID = entity.ScopeID,
                    Scope = Scope.Purpose,
                    ContractID = entity.ContractID,
                    Contract = Contract.ContractName,
                    BudgetBasisID = entity.BudgetBasisID,
                    BudgetBasis = BudgetBasis.BudgetBase,

                    //OPYearBudgetNaira = entity.OPYearBudgetNaira,
                    //OPYearBudgetDollar = entity.OPYearBudgetDollar,
                    //OPYearBudgetFDollar = entity.OPYearBudgetFDollar,
                    //NAPIMSBUDGETNaira = entity.NAPIMSBUDGETNaira,
                    //NAPIMSBUDGETDollar = entity.NAPIMSBUDGETDollar,
                    NAPIMSBUDGETFDollar = entity.NAPIMSBUDGETFDollar,
                    OPYearBudgetFDollar = 0,
                    Commitments = GetTotalSumCostBreakDownByBudgetBookId(Commitment.BudgetBookID),

                    //YYear = BBookFinanceYear.YYear,
                    //Q1FYLENaira = BBookFinanceYear.Q1FYLENaira,
                    //Q1FYLEDollar = BBookFinanceYear.Q1FYLEDollar,
                    //Q1FYLEFDollar = BBookFinanceYear.Q1FYLEFDollar,

                    //Q2FYLENaira = BBookFinanceYear.Q2FYLENaira,
                    //Q2FYLEDollar = BBookFinanceYear.Q2FYLEDollar,
                    //Q2FYLEFDollar = BBookFinanceYear.Q2FYLEFDollar,

                    //Q3FYLENaira = BBookFinanceYear.Q3FYLENaira,
                    //Q3FYLEDollar = BBookFinanceYear.Q3FYLEDollar,
                    //Q3FYLEFDollar = BBookFinanceYear.Q3FYLEFDollar,

                    //Q4FYLENaira = BBookFinanceYear.Q4FYLENaira,
                    //Q4FYLEDollar = BBookFinanceYear.Q4FYLEDollar,
                    //Q4FYLEFDollar = BBookFinanceYear.Q4FYLEFDollar,
                };
            }).ToList();
            return result;
        }

        private IEnumerable<BudgetBookViewModel> GetBudgetBooks(int? iYear)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            var BudgetBases = repoBudgetBasis.GetAll().Result.ToList();
            var Activities = repoActivity.GetAll().Result.ToList();
            var ActivityNames = repoActivityName.GetAll().Result.ToList();
            var Scopes = repoScope.GetAll().Result.ToList();
            var Contracts = repoContract.GetAll().Result.ToList();
            var UapCodes = repoUapCode.GetAll().Result.ToList();
            var UapRollUpCodes = repoUapRollUpCode.GetAll().Result.ToList();
            var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
            var WBSes = repoWBS.GetAll().Result.ToList();
            var CapexOpexs = repoCapexOpex.GetAll().Result.ToList();

            var result = repo.GetAll().Result.Where(o => o.YYear == iYyear).OrderBy(o => o.ID).ToList().Select(entity =>
            {
                var BudgetBook = new BudgetBook();
                var BudgetBasis = new BudgetBasis();
                var Activity = new Activity();
                var ActivityName = new ActivityType();
                var Scope = new Scope();
                var Contract = new Contract();
                var UapCode = new UapCode();
                var Uaprollupcode = new UapRollUpCode();
                var ActivityCode = new ActivityCode();
                var wbs = new WBS();
                var CapexOpex = new CXOX();

                if (BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => entity.BudgetBasisID == c.ID);
                if (Activities.FirstOrDefault(c => entity.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => entity.ActivityID == c.ID);
                if (ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => entity.ActivityTypeID == c.ID);
                if (Scopes.FirstOrDefault(c => entity.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => entity.ScopeID == c.ID);
                if (Contracts.FirstOrDefault(c => entity.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => entity.ContractID == c.ID);
                if (UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => entity.UapCodeID == c.ID);
                if (UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCodeID == c.ID);
                if (ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => entity.ActivityCodeID == c.ID);
                if (WBSes.FirstOrDefault(c => entity.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => entity.wbsID == c.ID);
                if (CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => entity.CapexOpexID == c.ID);

                return new BudgetBookViewModel
                {
                    ID = entity.ID,
                    CapexOpexID = entity.CapexOpexID,
                    CapexOpex = CapexOpex.CapexOpex,
                    DirectAllocated = entity.DirectAllocated,
                    sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(entity.DirectAllocated))).ToString(),
                    UapCodeID = entity.UapCodeID,
                    UapCode = UapCode.UapCodeDesc,
                    UapRollUpCodeID = entity.UapRollUpCodeID,
                    UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                    ActivityTypeID = entity.ActivityTypeID,
                    ActivityType = ActivityName.ActivityName,
                    ActivityCode = ActivityCode.ActivityCodeDesc,
                    wbsID = entity.wbsID,
                    CostObject = wbs.CostObjects,
                    ActivityID = entity.ActivityID,
                    Activity = Activity.Description,

                    ActivityCodeID = entity.ActivityCodeID,
                    ActivityOwnerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().ID,
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,

                    ActivityCodeLineManager = ActivityCode.LineManager.FullName,
                    ScopeID = entity.ScopeID,
                    Scope = Scope.Purpose,
                    ContractID = entity.ContractID,
                    Contract = Contract.ContractName,
                    BudgetBasisID = entity.BudgetBasisID,
                    BudgetBasis = BudgetBasis.BudgetBase,

                    //OPYearBudgetNaira = entity.OPYearBudgetNaira,
                    //OPYearBudgetDollar = entity.OPYearBudgetDollar,
                    //OPYearBudgetFDollar = entity.OPYearBudgetFDollar,
                    NAPIMSBUDGETNaira = entity.NAPIMSBUDGETNaira,
                    NAPIMSBUDGETDollar = entity.NAPIMSBUDGETDollar,
                    NAPIMSBUDGETFDollar = entity.NAPIMSBUDGETFDollar,
                    //YYear = BBookFinanceYear.YYear,
                    //Q1FYLENaira = BBookFinanceYear.Q1FYLENaira,
                    //Q1FYLEDollar = BBookFinanceYear.Q1FYLEDollar,
                    //Q1FYLEFDollar = BBookFinanceYear.Q1FYLEFDollar,

                    //Q2FYLENaira = BBookFinanceYear.Q2FYLENaira,
                    //Q2FYLEDollar = BBookFinanceYear.Q2FYLEDollar,
                    //Q2FYLEFDollar = BBookFinanceYear.Q2FYLEFDollar,

                    //Q3FYLENaira = BBookFinanceYear.Q3FYLENaira,
                    //Q3FYLEDollar = BBookFinanceYear.Q3FYLEDollar,
                    //Q3FYLEFDollar = BBookFinanceYear.Q3FYLEFDollar,

                    //Q4FYLENaira = BBookFinanceYear.Q4FYLENaira,
                    //Q4FYLEDollar = BBookFinanceYear.Q4FYLEDollar,
                    //Q4FYLEFDollar = BBookFinanceYear.Q4FYLEFDollar,
                };
            }).ToList();
            return result;
        }


        //TODO: To be moved to common library
        public decimal GetTotalSumCostBreakDownByBudgetBookId(long? id)
        {
            var totSum = GetCostBreakdownByBudgetBookId(id);
            decimal? CurentTotalExpendedFromBudgetLine = 0.0M;
            foreach (var t in totSum)
            {
                CurentTotalExpendedFromBudgetLine += t.Calculated;
            }

            return (decimal)CurentTotalExpendedFromBudgetLine;
        }

        //TODO: To be moved to common library
        public IEnumerable<ActivityDetailsViewModel> GetCostBreakdownByBudgetBookId(long? id)
        {
            //TODO: do something about the hardcoded "US Dollar" currency nane here.
            var Currenci = repoCurrencies.GetAll().Result.ToList();
            var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();

            //var result = repoCostBreakdown.GetAll().Result.Where(o => o.iYear == DateTime.Now.Year).ToList().Select(entity =>
            var result = repoCostBreakdown.GetAll().Result.Where(o => o.BudgetBookID == id).ToList().Select(entity =>
            {
                var currenci = Currenci.First(c => entity.CurrenciesID == c.ID);

                return new ActivityDetailsViewModel
                {
                    ID = entity.ID,
                    BudgetBookCommitmentsID = entity.BudgetBookCommitmentsID,
                    Description = entity.Description,
                    Quantity = entity.Quantity,
                    Rate = entity.Rate,
                    FixedExchangeRate = entity.FixedExchangeRate,
                    CurrenciesID = entity.CurrenciesID,
                    iYear = entity.iYear,
                    BudgetBookID = entity.BudgetBookID,
                    CurrencyName = currenci.CurrencyName,
                    Calculated = (currenci.CurrencyName == "US Dollar")
                    ? (entity.Rate * entity.Quantity) : (entity.FixedExchangeRate == 0.0M || entity.FixedExchangeRate == null)
                    ? (entity.Rate / Exchange.FloatingExchangeRate) * entity.Quantity : (entity.Rate / entity.FixedExchangeRate) * entity.Quantity,
                };
            }).ToList();
            return result;
        }

        public IEnumerable<ActivityDetailsViewModel> GetBudgetBookCostBreakdown()
        {
            //TODO: do something about the hardcoded "US Dollar" currency nane here.
            var Currenci = repoCurrencies.GetAll().Result.ToList();
            var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();

            var result = repoCostBreakdown.GetAll().Result.Where(o => o.iYear == DateTime.Now.Year).ToList().Select(entity =>
              {
                  var currenci = Currenci.First(c => entity.CurrenciesID == c.ID);

                  return new ActivityDetailsViewModel
                  {
                      ID = entity.ID,
                      BudgetBookCommitmentsID = entity.BudgetBookCommitmentsID,
                      Description = entity.Description,
                      Quantity = entity.Quantity,
                      Rate = entity.Rate,
                      FixedExchangeRate = entity.FixedExchangeRate,
                      CurrenciesID = entity.CurrenciesID,
                      iYear = entity.iYear,
                      BudgetBookID = entity.BudgetBookID,
                      CapexOpexID = repo.GetById(entity.BudgetBookID).Result.CapexOpexID,
                      CurrencyName = currenci.CurrencyName,
                      Calculated = (currenci.CurrencyName == "US Dollar")
                      ? (entity.Rate * entity.Quantity) : (entity.FixedExchangeRate == 0.0M || entity.FixedExchangeRate == null)
                      ? (entity.Rate / Exchange.FloatingExchangeRate) * entity.Quantity : (entity.Rate / entity.FixedExchangeRate) * entity.Quantity,
                  };
              }).ToList();
            return result;
        }

        public decimal GetTotalSumBudgetBookCostBreakDownByCapex()
        {
            var totSum = GetBudgetBookCostBreakdown().Where(o => o.CapexOpexID == 3);
            decimal? CurentTotalExpendedFromBudgetLine = 0.0M;
            foreach (var t in totSum)
            {
                CurentTotalExpendedFromBudgetLine += t.Calculated;
            }

            return (decimal)CurentTotalExpendedFromBudgetLine;
        }

        public decimal GetTotalSumBudgetBookCostBreakDownByOpex()
        {
            var totSum = GetBudgetBookCostBreakdown().Where(o => o.CapexOpexID == 2);
            decimal? CurentTotalExpendedFromBudgetLine = 0.0M;
            foreach (var t in totSum)
            {
                CurentTotalExpendedFromBudgetLine += t.Calculated;
            }

            return (decimal)CurentTotalExpendedFromBudgetLine;
        }

        public async Task<IActionResult> PopupWindow(long id)
        {
            BudgetBook entity = await repo.GetById(id);
            BudgetBookViewModel model = new BudgetBookViewModel();

            if (entity != null)
            {
                model.ID = entity.ID;
            }

            return PartialView("_BudgetBookNew", model);
        }

        public IActionResult LookUp(int? iYear)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                //lstBudgetBooks = GetBudgetBooks(iYyear)
            };

            ViewData["ActivityID"] = new SelectList((repoActivity.GetAll() != null) ? repoActivity.GetAll().Result.ToList() : null, "ID", "Description");
            ViewData["ActivityCodeID"] = new SelectList((repoActivityCode.GetAll() != null) ? repoActivityCode.GetAll().Result.ToList() : null, "ID", "ActivityCodeDesc");
            ViewData["ActivityTypeID"] = new SelectList((repoActivityName.GetAll() != null) ? repoActivityName.GetAll().Result.ToList() : null, "ID", "ActivityName");
            ViewData["BudgetBasisID"] = new SelectList((repoBudgetBasis.GetAll() != null) ? repoBudgetBasis.GetAll().Result.ToList() : null, "ID", "BudgetBase");
            ViewData["ContractID"] = new SelectList((repoContract.GetAll() != null) ? repoContract.GetAll().Result.ToList() : null, "ID", "ContractName");
            ViewData["ScopeID"] = new SelectList((repoScope.GetAll() != null) ? repoScope.GetAll().Result.ToList() : null,  "ID", "Purpose");
            //ViewData["SponsorID"] = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Result.ToList() : null,  "ID", "FullName");
            //ViewData["ActivityOwnerID"] = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Result.ToList() : null, "ID", "FullName");
            ViewData["UapCodeID"] = new SelectList((repoUapCode.GetAll() != null) ? repoUapCode.GetAll().Result.ToList() : null,  "ID", "UapCodeDesc");
            ViewData["UapRollUpCodeID"] = new SelectList((repoUapRollUpCode.GetAll() != null) ? repoUapRollUpCode.GetAll().Result.ToList() : null,  "ID", "UapRollUpCodeDesc");
            ViewData["wbsID"] = new SelectList((repoWBS.GetAll() != null) ? repoWBS.GetAll().Result.ToList() : null,  "ID", "CostObjects");
            ViewData["CapexOpex"] = new SelectList((repoCapexOpex.GetAll() != null) ? repoCapexOpex.GetAll().Result.ToList() : null, "ID", "CapexOpex");
            //ViewData["CapexOpex"] = new SelectList(repoCapexOpex.GetAll()?.ToList(), "ID", "CapexOpex");

            return View(model);
        }

        public IActionResult LoadData(long? IdActivityOwner, long? IdCapexOpex, long? IdActivityCode, long? IdActivityName, long? IdScope, long? IdBudgetBasis, int? iYear)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault(); // Skip number of Rows count  
                var length = Request.Form["length"].FirstOrDefault(); // Paging Length 10,20  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(); // Sort Column Name  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(); // Sort Column Direction (asc, desc)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper(); // Search Value from (Search box)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0; //Paging Size (10, 20, 50,100)  
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //IEnumerable<BudgetBookViewModel> customerData = Enumerable.Empty<BudgetBookViewModel>();

                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

                var customerData = GetBudgetBooks(iYyear, IdCapexOpex, IdActivityOwner, IdActivityCode, IdActivityName, IdScope, IdBudgetBasis);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Activity.ToUpper().Contains(searchValue) || m.ActivityOwner.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts    
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging 
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data                 
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
        }


        public IActionResult LoadData2(long? IdActivityOwner, long? IdActivityCode, int? iYear)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault(); // Skip number of Rows count  
                var length = Request.Form["length"].FirstOrDefault(); // Paging Length 10,20  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(); // Sort Column Name  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(); // Sort Column Direction (asc, desc)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper(); // Search Value from (Search box)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0; //Paging Size (10, 20, 50,100)  
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                var customerData = GetBudgetBooks2(iYyear, IdActivityOwner, IdActivityCode);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Activity.ToUpper().Contains(searchValue) || m.ActivityOwner.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts    
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging 
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data                 
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public IActionResult LoadBudgetBookByYear(int? iYear)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault(); // Skip number of Rows count  
                var length = Request.Form["length"].FirstOrDefault(); // Paging Length 10,20  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(); // Sort Column Name  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(); // Sort Column Direction (asc, desc)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper(); // Search Value from (Search box)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0; //Paging Size (10, 20, 50,100)  
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //IEnumerable<BudgetBookViewModel> customerData = Enumerable.Empty<BudgetBookViewModel>();

                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

                var customerData = GetBudgetBooks(iYyear);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Activity.ToUpper().Contains(searchValue.ToUpper()) 
                                                                                            || m.ActivityOwner.ToUpper().Contains(searchValue.ToUpper())
                                                                                            || m.CostObject.ToUpper().Contains(searchValue.ToUpper())
                                                                                            || m.ActivityCode.ToUpper().Contains(searchValue.ToUpper())
                                                                                            || m.Scope.ToUpper().Contains(searchValue.ToUpper())
                                                                                            || m.Contract.ToUpper().Contains(searchValue.ToUpper())
                                                                                            || m.BudgetBasis.ToUpper().Contains(searchValue.ToUpper())); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts    
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging 
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data                 
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.Fail(ex.TargetSite.Name + "\n \n" + ex.StackTrace + "\n \n" + ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IActionResult> DeleteBudgetLineItem(long? id)
        {
            if (id == null)
            {
                TempData["Message"] = id + " Not Found!!!";
                return RedirectToAction("ActivityOwner");
            }

            var entity = await repo.GetById(id);

            //Check if this entity's ID is found in BudgetBookCommitment. 
            var iCount = repoBudgetBookCommitments.GetAll().Result.Where(o => o.BudgetBookID == entity.ID).Count();
            if (iCount > 0)
            {
                TempData["Message"] = "Budget Book already in use. Some Commitments have already been drawn from the NAPIMS value.";
                return RedirectToAction("ActivityOwner");
            }

            if (entity == null)
            {
                TempData["Message"] = id + " Not Found!!!";
                return RedirectToAction("ActivityOwner");
            }
            else await repo.Delete(entity);

            return View(entity);
        }


        #region ======================= CRUD =============================

        public IActionResult NewBudgetBook()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();
            //model.oBudgetBook = GetBudgetBook(id).FirstOrDefault();

            ViewBag.DirectAllocated = new SelectList(RolesManager.GetDirectAllocated(), "Value", "Text");
            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex");
            ViewBag.ActivityName = new SelectList(repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName).ToList(), "ID", "ActivityName");
            ViewBag.UapCodeID = new SelectList((repoUapCode.GetAll() != null) ? repoUapCode.GetAll().Result.OrderBy(o => o.UapCodeDesc).ToList() : null, "ID", "UapCodeDesc");
            ViewBag.UapRollUpCodeID = new SelectList((repoUapRollUpCode.GetAll() != null) ? repoUapRollUpCode.GetAll().Result.OrderBy(o => o.UapRollUpCodeDesc).ToList() : null, "ID", "UapRollUpCodeDesc");

            ViewBag.Activity = new SelectList((repoActivity.GetAll() != null) ? repoActivity.GetAll().Result.OrderBy(o => o.Description).ToList().ToList() : null, "ID", "Description");
            ViewBag.ActivityCode = new SelectList((repoActivityCode.GetAll() != null) ? repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList() : null, "ID", "ActivityCodeDesc");
            ViewBag.BudgetBasis = new SelectList((repoBudgetBasis.GetAll() != null) ? repoBudgetBasis.GetAll().Result.OrderBy(o => o.BudgetBase).ToList() : null, "ID", "BudgetBase");
            ViewBag.Contract = new SelectList((repoContract.GetAll() != null) ? repoContract.GetAll().Result.OrderBy(o => o.ContractName).ToList() : null, "ID", "ContractName");
            ViewBag.Scopes = new SelectList((repoScope.GetAll() != null) ? repoScope.GetAll().Result.OrderBy(o => o.Purpose).ToList() : null, "ID", "Purpose");
            //ViewBag.Sponsors = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList() : null, "ID", "FullName");
            //ViewBag.ActivityOwners = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList() : null, "ID", "FullName");
            ViewBag.WBS = new SelectList((repoWBS.GetAll() != null) ? repoWBS.GetAll().Result.ToList() : null, "ID", "CostObjects");

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_BudgetBookNew", model);
        }

        public IActionResult EditBudgetBook(long? id, int? iYear)
        {
            if (id == null)
            {
                return NotFound();
            }

            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;


            CommitmentControlViewModel model = new CommitmentControlViewModel();
            model.oBudgetBook = GetBudgetBook(id).FirstOrDefault();

            ViewBag.DirectAllocated = new SelectList(RolesManager.GetDirectAllocated(), "Value", "Text", model.oBudgetBook.DirectAllocated);
            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex", model.oBudgetBook.CapexOpexID);
            ViewBag.ActivityName = new SelectList(repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName).ToList(), "ID", "ActivityName", model.oBudgetBook.ActivityTypeID);
            ViewBag.UapCodeID = new SelectList((repoUapCode.GetAll() != null) ? repoUapCode.GetAll().Result.OrderBy(o => o.UapCodeDesc).ToList() : null, "ID", "UapCodeDesc", model.oBudgetBook.UapCodeID);
            ViewBag.UapRollUpCodeID = new SelectList((repoUapRollUpCode.GetAll() != null) ? repoUapRollUpCode.GetAll().Result.OrderBy(o => o.UapRollUpCodeDesc).ToList() : null, "ID", "UapRollUpCodeDesc", model.oBudgetBook.UapRollUpCodeID);

            ViewBag.Activity = new SelectList((repoActivity.GetAll() != null) ? repoActivity.GetAll().Result.OrderBy(o => o.Description).ToList().ToList() : null, "ID", "Description", model.oBudgetBook.ActivityID);
            ViewBag.ActivityCode = new SelectList((repoActivityCode.GetAll() != null) ? repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList() : null, "ID", "ActivityCodeDesc", model.oBudgetBook.ActivityCodeID);
            ViewBag.BudgetBasis = new SelectList((repoBudgetBasis.GetAll() != null) ? repoBudgetBasis.GetAll().Result.OrderBy(o => o.BudgetBase).ToList() : null, "ID", "BudgetBase", model.oBudgetBook.BudgetBasisID);
            ViewBag.Contract = new SelectList((repoContract.GetAll() != null) ? repoContract.GetAll().Result.OrderBy(o => o.ContractName).ToList() : null, "ID", "ContractName", model.oBudgetBook.ContractID);
            ViewBag.Scopes = new SelectList((repoScope.GetAll() != null) ? repoScope.GetAll().Result.OrderBy(o => o.Purpose).ToList() : null, "ID", "Purpose", model.oBudgetBook.ScopeID);
            ViewBag.Sponsors = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList() : null, "ID", "FullName", model.oBudgetBook.SponsorID);
            ViewBag.ActivityOwners = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList() : null, "ID", "FullName", model.oBudgetBook.ActivityOwnerID);
            ViewBag.WBS = new SelectList((repoWBS.GetAll() != null) ? repoWBS.GetAll().Result.ToList() : null, "ID", "CostObjects", model.oBudgetBook.wbsID);

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_BudgetBookNew", model);
        }

        public async Task<IActionResult> CreateUpdateBudgetBook(CommitmentControlViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            if (LoginUser != null)
            {
                bool isNew = !model.oBudgetBook.ID.HasValue;
                BudgetBook entity = isNew ? new BudgetBook { AddedDate = DateTime.Today.Date, ModifiedDate = DateTime.Today.Date } : await repo.GetById(model.oBudgetBook.ID);

                entity.CapexOpexID = model.oBudgetBook.CapexOpexID;
                entity.DirectAllocated = model.oBudgetBook.DirectAllocated;
                entity.UapCodeID = model.oBudgetBook.UapCodeID;
                entity.UapRollUpCodeID = model.oBudgetBook.UapRollUpCodeID;
                entity.ActivityTypeID = model.oBudgetBook.ActivityTypeID;
                entity.ActivityCodeID = model.oBudgetBook.ActivityCodeID;
                entity.wbsID = model.oBudgetBook.wbsID;
                entity.ActivityID = model.oBudgetBook.ActivityID;
                entity.ScopeID = model.oBudgetBook.ScopeID;
                entity.ContractID = model.oBudgetBook.ContractID;
                entity.BudgetBasisID = model.oBudgetBook.BudgetBasisID;
                //entity.NAPIMSBUDGETNaira = model.oBudgetBook.NAPIMSBUDGETNaira;
                //entity.NAPIMSBUDGETDollar = model.oBudgetBook.NAPIMSBUDGETDollar;
                entity.NAPIMSBUDGETFDollar = model.oBudgetBook.NAPIMSBUDGETFDollar;
                entity.YYear = DateTime.Now.Year;

                if (isNew)
                {
                    long? iRet = await repo.Insert(entity);
                }
                else
                {
                    entity.ModifiedDate = DateTime.Today.Date;
                    bool bRet = await repo.Update(entity);
                }
            }
            //}
            return RedirectToAction("Index");
        }

        #endregion
    }
}
