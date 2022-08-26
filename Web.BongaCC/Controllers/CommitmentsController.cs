using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EF.BongaCC.Core.Model;
using Web.BongaCC.ViewModels;
using EF.BongaCC.Data.Repository;
using Microsoft.AspNetCore.Http;
using System.IO;
using Web.BongaCC.Codes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Mvc.Filters;
using Kendo.Mvc.UI;
using Web.BongaCC.Extensions;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

//Sending email
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MimeKit.Text;

using Web.BongaCC.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using App.Services;
using Kendo.Mvc.Extensions;
using System.Globalization;
using System.Text;
using DinkToPdf.Contracts;
using DinkToPdf;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection.Metadata.Ecma335;


namespace Web.BongaCC.Controllers
{
    //public enum enuApproval
    //{
    //    ApprovedAsPresented = 1,
    //    ApprovedWithModification = 2,
    //    ProvisionalApproval = 3,
    //    SteppedDown = 4,
    //    Declined = 5,
    //    DefferedFurtherDocumentationRequired = 6,
    //    PostPoned = 7,
    //    TBD = 8,
    //    Draft = 9,
    //    LineManagerReview = 10,
    //    PendingCCPanelSession = 11,
    //}

    public partial class CommitmentsController : Controller
    {
        private readonly IOptions<appSettingsModel> appSettings;
        //private IConfiguration configuration;

        private readonly IRepository<ActivityDetails> repoCostBreakdown;
        private readonly IRepository<Asset> repoAsset;

        private readonly IRepository<ContractProcurementVehicle> repoVehicle;
        private readonly IRepository<PlannedEmmergency> repoPlannedEmmergency;
        private readonly IRepository<PurchasingGroup> repoPurchaseGroup;
        private readonly IRepository<RequestStatus> repoStatus;

        private readonly IRepository<Department> repoDept;
        private readonly IRepository<Facility> repoFacility;
        private readonly IRepository<AppUsers> repoUsers;
        private readonly IRepository<Team> repoTeam;
        private readonly IRepository<WBS> repoWBS;
        private readonly IRepository<ExchangeRate> repoExchRate;
        private readonly IRepository<FileUpload> repoFileUpload;

        //BudgetBook
        private readonly IRepository<BudgetBook> repoBudgetBook;
        private readonly IRepository<BudgetBookCommitments> repoBudgetBookCommitments;
        private readonly IRepository<BudgetBasis> repoBudgetBasis;
        private readonly IRepository<Activity> repoActivity;
        private readonly IRepository<ActivityType> repoActivityName;
        private readonly IRepository<Scope> repoScope;
        private readonly IRepository<Contract> repoContract;
        private readonly IRepository<UapCode> repoUapCode;
        private readonly IRepository<UapRollUpCode> repoUapRollUpCode;
        private readonly IRepository<ActivityCode> repoActivityCode;
        private readonly IRepository<ActivityCodeWorkStream> repoActivityCodeWorkStream;
        private readonly IRepository<CXOX> repoCapexOpex;
        private readonly IRepository<Currencies> repoCurrencies;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly HttpContext currentContext;

        private readonly IEmailSender _emailSender;
        private IConverter _converter;

        private readonly ISession _session;
        public ISession Session { get { return _session; } }

        //private readonly ILogger<CommitmentsController> _logger;
        // Get the default form options so that we can use them to set the default limits for
        // request body data
        //private static readonly FormOptions _defaultFormOptions = new FormOptions();


        //IRepository<ApprovalDecision> repoApprovalDecision,
        //this.repoApprovalDecision = repoApprovalDecision; 
        public CommitmentsController(IRepository<ContractProcurementVehicle> repoVehicle, 
            IRepository<Department> repoDept, IRepository<Facility> repoFacility, IRepository<PlannedEmmergency> repoPlannedEmmergency, 
            IRepository<PurchasingGroup> repoPurchaseGroup, IRepository<RequestStatus> repoStatus, IRepository<AppUsers> repoUsers, 
            IRepository<Team> repoTeam, IRepository<WBS> repoWBS, IRepository<Asset> repoAsset, IRepository<ExchangeRate> repoExchRate, 
            IRepository<ActivityDetails> repoCostBreakdown, IRepository<FileUpload> repoFileUpload, IRepository<BudgetBook> repoBudgetBook,
            IRepository<BudgetBasis> repoBudgetBasis, IRepository<Activity> repoActivity, IRepository<ActivityType> repoActivityName, 
            IRepository<Scope> repoScope, IRepository<Contract> repoContract, IRepository<UapCode> repoUapCode, IRepository<UapRollUpCode> repoUapRollUpCode, 
            IRepository<ActivityCode> repoActivityCode, IRepository<ActivityCodeWorkStream> repoActivityCodeWorkStream, IRepository<CXOX> repoCapexOpex, 
            IRepository<Currencies> repoCurrencies, IRepository<BudgetBookCommitments> repoBudgetBookCommitments, IHttpContextAccessor httpContextAccessor, 
            IWebHostEnvironment env, IEmailSender emailSender, IOptions<appSettingsModel> app, IConverter converter)
        {
            this.repoAsset = repoAsset; this.repoVehicle = repoVehicle; this.repoDept = repoDept; this.repoFacility = repoFacility;
            this.repoPlannedEmmergency = repoPlannedEmmergency; this.repoPurchaseGroup = repoPurchaseGroup; this.repoStatus = repoStatus;
            this.repoUsers = repoUsers; this.repoTeam = repoTeam; this.repoWBS = repoWBS; this.repoExchRate = repoExchRate;
            this.repoCostBreakdown = repoCostBreakdown; this.repoFileUpload = repoFileUpload;
            this.repoBudgetBook = repoBudgetBook; this.repoBudgetBasis = repoBudgetBasis; this.repoActivity = repoActivity; 
            this.repoActivityCodeWorkStream = repoActivityCodeWorkStream;
            this.repoActivityName = repoActivityName; this.repoScope = repoScope; this.repoContract = repoContract;
            this.repoUapCode = repoUapCode; this.repoUapRollUpCode = repoUapRollUpCode; this.repoActivityCode = repoActivityCode; 
            this.repoCapexOpex = repoCapexOpex; this.repoCurrencies = repoCurrencies; this.repoBudgetBookCommitments = repoBudgetBookCommitments;

            //this.repoApprovalDecision = repoApprovalDecision;

            _session = httpContextAccessor.HttpContext.Session;
            currentContext = httpContextAccessor.HttpContext;
            _env = env;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            appSettings = app;
            _converter = converter;
        }

        #region ==================== Views that land on Pages =======================

        // The Index View is for Focal Points and Admins
        [HttpGet]
        public IActionResult Index()
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            if (LoginUser != null)
            {
                if (LoginUser.RoleId == (int)enuRole.ActivityOwner) return RedirectToAction("ActivityOwner");
                else if (LoginUser.RoleId == (int)enuRole.LineManager) return RedirectToAction("LineManager");
                else if (LoginUser.RoleId == (int)enuRole.AccountableManager) return RedirectToAction("Review");
                else if (LoginUser.RoleId == (int)enuRole.Corporate) return RedirectToAction("Corporate"); 
            }
            else
            {
                return View("ErrorLogin"); //Return to you are not authorised to view this application, contact the administrator
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            //TODO: Note this is where the dropdownlist on th emain page derives all its data.
            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle"); //Comitmntno
            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");

            //ViewBag.vehicleID = new SelectList((repoVehicle.GetAll().Result != null) ? repoVehicle.GetAll().Result.OrderBy(o => o.VehicleName).ToList() : null, "ID", "VehicleName");
            //ViewBag.groupID = new SelectList((repoPurchaseGroup.GetAll().Result != null) ? repoPurchaseGroup.GetAll().Result.OrderBy(o => o.GroupName).ToList() : null, "ID", "GroupName");
            //ViewBag.teamID = new SelectList((repoTeam.GetAll().Result != null) ? repoTeam.GetAll().Result.OrderBy(o => o.TeamName).ToList() : null, "ID", "TeamName");
            //ViewBag.typeID = new SelectList((repoPlannedEmmergency.GetAll().Result != null) ? repoPlannedEmmergency.GetAll().Result.OrderBy(o => o.PlanEmmerType).ToList() : null, "ID", "PlanEmmerType");
            //ViewBag.statusID = new SelectList((repoStatus.GetAll().Result != null) ? repoStatus.GetAll().Result.OrderBy(o => o.ReqstStatus).ToList() : null, "ID", "ReqstStatus");

            //int? iYear = DateTime.Today.Year;
            return View(model);
        }

        public IActionResult BudgetBook(int? iYear)
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();
            //BudgetBook will be available only on Monday 12noon to Friday 4PM in the evening
            if ((DateTime.Today.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour > 11) ||
                (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday) ||
                (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday) ||
                (DateTime.Today.DayOfWeek == DayOfWeek.Thursday) ||
                (DateTime.Today.DayOfWeek == DayOfWeek.Friday && DateTime.Now.Hour <= 16))
            {
                model.CapexOpexList = repoCapexOpex.GetAll().Result.ToList();
                model.CapexOpexList.Insert(0, new CXOX { ID = 0, CapexOpex = "Select..." });
                ViewBag.CapexOpexBB = model.CapexOpexList;

                var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
                model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();
            }
            else
            {
                TempData["Message"] = "Budget Book currently not available. It is opened between Monday 12noon and Friday 4PM weekly.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> CostBreakdown(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault(),
            };
            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id);
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID);
            model.LstUploadFiles = await GetUploadedFiles(id);

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            model.lstCurrencies = repoCurrencies.GetAll().Result.ToList();
            ViewBag.Currency = model.lstCurrencies;

            //decimal? CurentTotalExpendedFromBudgetLine = Math.Round(GetTotalSumCostBreakDownByCommitmentId(id), 2);

            if (Math.Round(model.oBudgetBookCommitment.PRValue, 2) != Math.Round(GetTotalSumCostBreakDownByCommitmentId(id), 2))
            {
                var message = "Dear " + LoginUser.FullName + ", please note that your PR Value and total commitment breakdown are not equal. The system will not allow you forward this request to Activity Owner until the values are equal.";
                ViewBag.PRCommitmentMessage = message;
                ViewBag.PRCommitmentMessage2 = message;
            }

            if (model == null)
            {
                return NotFound();
            }

            //ViewBag.Message = TempData["Message"].ToString();

            return View(model);
        }

        public IActionResult ActivityOwner()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle");
            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");

            return View(model);
        }

        [HttpGet]
        public IActionResult LineManager()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            //TODO: Note this is where the dropdownlist on th emain page derives all its data.
            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle");
            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");
            //ViewBag.Threshold = string.Format("{0:c}", appSettings.Value.Threshold);
            ViewBag.Threshold = appSettings.Value.Threshold;

            return View(model);
        }

        [HttpGet]
        public IActionResult Review()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();
            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle");
            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");

            return View(model);
        }

        [HttpGet]
        public IActionResult Corporate()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();
            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle");
            ViewBag.User = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Corporate).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");


            //This is for previewing Budgetbook
            ViewBag.DirectAllocated = new SelectList(RolesManager.GetDirectAllocated(), "Value", "Text");
            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex");
            ViewBag.ActivityName = new SelectList(repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName).ToList(), "ID", "ActivityName");

            ViewBag.UapCodeID = new SelectList((repoUapCode.GetAll().Result != null) ? repoUapCode.GetAll().Result.OrderBy(o => o.UapCodeDesc).ToList() : null, "ID", "UapCodeDesc");
            ViewBag.UapRollUpCodeID = new SelectList((repoUapRollUpCode.GetAll().Result != null) ? repoUapRollUpCode.GetAll().Result.OrderBy(o => o.UapRollUpCodeDesc).ToList() : null, "ID", "UapRollUpCodeDesc");

            ViewBag.Activity = new SelectList((repoActivity.GetAll().Result != null) ? repoActivity.GetAll().Result.OrderBy(o => o.Description).ToList() : null, "ID", "Description");
            ViewBag.ActivityCode = new SelectList((repoActivityCode.GetAll().Result != null) ? repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList() : null, "ID", "ActivityCodeDesc");
            ViewBag.BudgetBasis = new SelectList((repoBudgetBasis.GetAll().Result != null) ? repoBudgetBasis.GetAll().Result.OrderBy(o => o.BudgetBase).ToList() : null, "ID", "BudgetBase");
            ViewBag.Contract = new SelectList((repoContract.GetAll().Result != null) ? repoContract.GetAll().Result.OrderBy(o => o.ContractName).ToList() : null, "ID", "ContractName");
            ViewBag.Scopes = new SelectList((repoScope.GetAll().Result != null) ? repoScope.GetAll().Result.OrderBy(o => o.Purpose).ToList() : null, "ID", "Purpose");
            //ViewBag.Sponsors = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList() : null, "ID", "FullName");
            //ViewBag.ActivityOwners = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList() : null, "ID", "FullName");
            //ViewBag.LineManagers = new SelectList((repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.LineManager) != null) ? repoUsers.GetAll().Result.OrderBy(o => o.FullName).Where(o => o.RoleId == (int)enuRole.LineManager).ToList() : null, "ID", "FullName");
            ViewBag.WBS = new SelectList((repoWBS.GetAll().Result != null) ? repoWBS.GetAll().Result.OrderBy(o => o.CostObjectsDescription).ToList() : null, "ID", "CostObjects");

            //var CapexOpex = repo.GetAll().Result.Where(t => t.YYear == DateTime.Today.Year);
            //var Capex = CapexOpex.Where(o => o.CapexOpex.CapexOpex.Contains("Capex")).Sum(t => t.NAPIMSBUDGETFDollar);
            //var Opex = CapexOpex.Where(o => o.CapexOpex.CapexOpex.Contains("Opex")).Sum(t => t.NAPIMSBUDGETFDollar);

            //var CapexExp = GetTotalSumBudgetBookCostBreakDownByCapex();
            //var OpexExp = GetTotalSumBudgetBookCostBreakDownByOpex();

            //var CapexBal = Capex - CapexExp;
            //var OpexBal = Opex - OpexExp;

            //ViewBag.Capex = stringRoutine.formatAsBankMoney("$", Capex);
            //ViewBag.Opex = stringRoutine.formatAsBankMoney("$", Opex);
            //ViewBag.CapexExp = stringRoutine.formatAsBankMoney("$", CapexExp);
            //ViewBag.OpexExp = stringRoutine.formatAsBankMoney("$", OpexExp);
            //ViewBag.CapexBal = stringRoutine.formatAsBankMoney("$", CapexBal);
            //ViewBag.OpexBal = stringRoutine.formatAsBankMoney("$", OpexBal);

            return View(model);
        }

        public IActionResult CCPSession()
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            if (LoginUser.RoleId != (int)enuRole.Admin)
            {
                return RedirectToAction("Index");
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                //lstBudgetBooks = GetBudgetBooks(iYear)
            };

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();
            //TODO: Note this is where the dropdownlist on th emain page derives all its data.
            //ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle"); //Comitmntno
            ViewBag.Threshold = string.Format("{0:c}", appSettings.Value.Threshold);

            return View(model);
        }

        [HttpGet]
        public IActionResult Reporters()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            ViewBag.Weeks = new SelectList(Computations.GetWeeksInYear().ToList(), "WeekNum", "Description");

            List<SelectListItem> ExportTypes = new List<SelectListItem>();
            ExportTypes.Add(new SelectListItem() { Text = exportType.A4.ToString(), Value = ((int)exportType.A4).ToString() });
            ExportTypes.Add(new SelectListItem() { Text = exportType.A3.ToString(), Value = ((int)exportType.A3).ToString() });

            ViewBag.ExportType = new SelectList(ExportTypes, "Value", "Text");
            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle");

            ViewBag.CapexOpexBB = new SelectList(repoCapexOpex.GetAll().Result.ToList(), "ID", "CapexOpex");

            return View(model);
        }

        public IActionResult ReportVariance()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();


            return View(model);
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
                    CapexOpexID = repoBudgetBook.GetById(entity.BudgetBookID).Result.CapexOpexID,
                    CurrencyName = currenci.CurrencyName,
                    Calculated = (currenci.CurrencyName == "US Dollar")
                    ? (entity.Rate * entity.Quantity) : (entity.FixedExchangeRate == 0.0M || entity.FixedExchangeRate == null)
                    ? (entity.Rate / Exchange.FloatingExchangeRate) * entity.Quantity : (entity.Rate / entity.FixedExchangeRate) * entity.Quantity,
                };
            }).ToList();
            return result;
        }


        public IActionResult Routing()
        {
            ViewBag.FocalPoint = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint || o.RoleId == (int)enuRole.Admin).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");
            ViewBag.ActivityOwner = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");
            ViewBag.LineManager = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");
            ViewBag.AccountableManager = new SelectList((from users in repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).OrderBy(o => o.FullName) select new { ID = users.ID, fullname = users.FullName }), "ID", "fullname");

            //Roles
            ViewBag.ActivityOwnerRoleId = (int)enuRole.ActivityOwner;
            ViewBag.LineManagerRoleId = (int)enuRole.LineManager;
            ViewBag.AccountableManagerRoleId = (int)enuRole.AccountableManager;

            return View();
        }

        #endregion

        #region ==================== Special Services Region ====================

        public string GetBaseUrl()
        {
            var request = currentContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }

        public IEnumerable<CurrenciesViewModel> GetCurrencies()
        {
            var result = repoCurrencies.GetAll().Result.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                return new CurrenciesViewModel
                {
                    ID = entity.ID,
                    Codes = entity.Codes,
                    Country = entity.Country,
                    CurrencyName = entity.CurrencyName,
                    Number = entity.Number,
                };
            }).ToList();
            return result;
        }

        //This is to get all the costbreakdown on all commitment control request on a Budget line (from BudgetBook)
        public IEnumerable<ActivityDetailsViewModel> GetCostBreakdownByBudgetBookId(long? id)
        {
            //TODO: do something about the hardcoded "US Dollar" currency name here.
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

        //This is to get the Costbreakdown on a given commitment control request
        private IEnumerable<ActivityDetailsViewModel> GetCostBreakdownByCommitmentId(long? id)
        {
            //TODO: do something about the hardcoded "US Dollar" currency nane here.
            var Currenci = repoCurrencies.GetAll().Result.ToList();

            var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();

            //var result = repoCostBreakdown.GetAll().Result.Where(o => o.iYear == DateTime.Now.Year).ToList().Select(entity =>
            var result = repoCostBreakdown.GetAll().Result.Where(o => o.BudgetBookCommitmentsID == id).ToList().Select(entity =>
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

        //This is to get the Costbreakdown on a single entry of a commtiment request, especially when editing a costbreakdown item
        private IEnumerable<ActivityDetailsViewModel> GetCostBreakdownById(long? id)
        {
            //TODO: do something about the hardcoded "US Dollar" currency nane here.
            var Currenci = repoCurrencies.GetAll().Result.ToList();
            var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();

            //var result = repoCostBreakdown.GetAll().Result.Where(o => o.iYear == DateTime.Now.Year).ToList().Select(entity =>
            var result = repoCostBreakdown.GetAll().Result.Where(o => o.ID == id).ToList().Select(entity =>
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

        public decimal GetTotalSumCostBreakDownByCommitmentId(long? id)
        {
            var totSum = GetCostBreakdownByCommitmentId(id);
            decimal? CurentTotalExpendedFromBudgetLine = 0.0M;
            foreach (var t in totSum)
            {
                CurentTotalExpendedFromBudgetLine += t.Calculated;
            }

            return (decimal)CurentTotalExpendedFromBudgetLine;
        }

        public decimal GetTotalSumCostBreakDownByBudgetBookId(long? id)
        {
            //var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            //var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();
            var totSum = GetCostBreakdownByBudgetBookId(id);
            decimal? CurentTotalExpendedFromBudgetLine = 0.0M;
            foreach (var t in totSum)
            {
                CurentTotalExpendedFromBudgetLine += t.Calculated;
                //CurentTotalExpendedFromBudgetLine += (t.CurrencyName == "US Dollar")
                //    ? (t.Rate * t.Quantity) : (t.FixedExchangeRate == 0.0M || t.FixedExchangeRate == null)
                //    ? (t.Rate / Exchange.FloatingExchangeRate) * t.Quantity : (t.Rate / t.FixedExchangeRate) * t.Quantity;
            }

            return (decimal)CurentTotalExpendedFromBudgetLine;
        }

        #endregion

        #region  ==================== Budget Book ====================

        private IEnumerable<BudgetBookViewModel> GetBudgetBooks(int? iYear)
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
            var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();

            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            var result = repoBudgetBook.GetAll().Result.Where(o => o.YYear == iYyear).OrderBy(o => o.ID).ToList().Select(entity =>
            {
                var BudgetBasis = new BudgetBasis(); //BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                var Activity = new Activity(); //Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                var ActivityName = new ActivityType(); //ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                var Scope = new Scope(); //Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                var Contract = new Contract(); //Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                var UapCode = new UapCode(); //UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                var Uaprollupcode = new UapRollUpCode();
                var ActivityCode = new ActivityCode(); //ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                var wbs = new WBS(); //WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                var CapexOpex = new CXOX();  //CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

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
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    ActivityOwnerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().ID,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    LineManagerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().ID,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,
                    SponsorID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().ID,

                    //ActivityCodeLineManager = ActivityCode.LineManager.FullName,
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
                    YYear = entity.YYear,
                };
            }).ToList();
            return result;
        }

        private async Task<IEnumerable<BudgetBookViewModel>> GetBudgetBook(long? id)
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
            //var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            
            List<BudgetBook> optimised = new List<BudgetBook>();
            var o = await repoBudgetBook.GetById(id);
            optimised.Add(o);

            //var result = repoBudgetBook.GetAll().Result.Where(c => c.ID == id).ToList().Select(entity =>
            var result = optimised.Select(entity =>
            {
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
                    ActivityOwner = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().FullName,
                    ActivityOwnerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.ActivityOwnerID).FirstOrDefault().ID,
                    LineManager = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().FullName,
                    LineManagerID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.LineManagerID).FirstOrDefault().ID,
                    Sponsor = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().FullName,
                    SponsorID = repoUsers.GetAll().Result.Where(t => t.ID == ActivityCode.AccountableManagerID).FirstOrDefault().ID,

                    ActivityCodeLineManager = ActivityCode.LineManager.FullName,
                    ScopeID = entity.ScopeID,
                    Scope = Scope.Purpose,
                    ContractID = entity.ContractID,
                    Contract = Contract.ContractName,
                    BudgetBasisID = entity.BudgetBasisID,
                    BudgetBasis = BudgetBasis.BudgetBase,

                    NAPIMSBUDGETFDollar = entity.NAPIMSBUDGETFDollar,
                    YYear = entity.YYear,
                    //OPYearBudgetNaira = entity.OPYearBudgetNaira,
                    //OPYearBudgetDollar = entity.OPYearBudgetDollar,
                    //OPYearBudgetFDollar = entity.OPYearBudgetFDollar,
                    //NAPIMSBUDGETNaira = entity.NAPIMSBUDGETNaira,
                    //NAPIMSBUDGETDollar = entity.NAPIMSBUDGETDollar,
                };
            }).ToList();
            return result;
        }

        #region  ==================== Budget Book Search Dropdownlists  ==================== 

        public JsonResult GetCapexOpex()
        {
            List<CXOX> list = repoCapexOpex.GetAll().Result.ToList();

            list.Insert(0, new CXOX { ID = 0, CapexOpex = "Select Capex/Opex..." });

            var result = new SelectList(list, "ID", "CapexOpex");
            return Json(result);
        }

        public JsonResult GetActivityOwnerByCapexOpex(long id)
        {
            //GetActivityOwnerByCapexOpex
            var iCurrentYear = DateTime.Today.Year;

            List<AppUsers> olist = new List<AppUsers>();
            List<AppUsers> list = new List<AppUsers>();

            var query = repoActivityCode.GetAll().Result.Join(repoBudgetBook.GetAll().Result
                .Where(c => c.YYear == iCurrentYear), actCode => actCode.ID, butbook => butbook.ActivityCodeID, (actCode, butbook) => new { actCode, butbook })
                .Where(t => t.butbook.CapexOpexID == id)
                .GroupBy(t => t.actCode.ActivityOwnerID)
                .Select(group => group.First());

            foreach (var b in query)
            {
                //Source: https://stackoverflow.com/questions/22534035/lambda-expression-to-return-one-result-for-each-distinct-value-in-list
                //var list = callList.GroupBy(x => x.ApplicationID).Select(x => x.First()).ToList();
                var r = repoUsers.GetAll().Result.Where(t => t.ID == b.actCode.ActivityOwnerID)
                .Select(o => new AppUsers
                {
                    ID = o.ID,
                    FullName = o.FullName
                });

                olist.Add(r.FirstOrDefault());
            }

            list.AddRange(olist.OrderBy(x => x.FullName));

            list.Insert(0, new AppUsers { ID = 0, FullName = "Select Activity Owner..." });
            var result = new SelectList(list, "ID", "FullName");
            return Json(result);
        }

        public IEnumerable<ActivityCodeViewModel> GetActivityCodes()
        {
            var ActivityOwners = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList();
            var LineManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).ToList();
            var AccountableManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList();

            try
            {
                var result = repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList().Select(entity =>
                {
                    var ActivityOwner = new AppUsers();
                    var LineManager = new AppUsers();
                    var AccountableManager = new AppUsers();

                    if (ActivityOwners.FirstOrDefault(c => entity.ActivityOwnerID == c.ID) != null) ActivityOwner = ActivityOwners.FirstOrDefault(c => entity.ActivityOwnerID == c.ID);
                    if (LineManagers.FirstOrDefault(c => entity.LineManagerID == c.ID) != null) LineManager = LineManagers.FirstOrDefault(c => entity.LineManagerID == c.ID);
                    if (AccountableManagers.FirstOrDefault(c => entity.AccountableManagerID == c.ID) != null) AccountableManager = AccountableManagers.FirstOrDefault(c => entity.AccountableManagerID == c.ID);

                    return new ActivityCodeViewModel
                    {
                        ID = entity.ID,
                        ActivityCodeDesc = entity.ActivityCodeDesc,
                        ActivityOwnerID = ActivityOwner.ID,
                        ActivityOwnerFullName = ActivityOwner.FullName,
                        LineManagerID = LineManager.ID,
                        LineManagerFullName = LineManager.FullName,
                        AccountableManagerID = AccountableManager.ID,
                        AccountableManagerFullName = AccountableManager.FullName,

                        //(entity.LineManagerID != null) ? repoUsers.GetById(entity.LineManagerID).FullName : "N/A",
                        //LineManager = repoUsers.GetById(entity.LineManagerID),
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetActivityCodeByActivityOwner(long? id)
        {
            List<ActivityCode> list = repoActivityCode.GetAll().Result.Where(c => c.ActivityOwnerID == id).ToList();
            list.Insert(0, new ActivityCode { ID = 0, ActivityCodeDesc = "Select Activity Code..." });

            var result = new SelectList(list, "ID", "ActivityCodeDesc");
            return Json(result);
        }

        public JsonResult GetActivityNameByActivityOwner(long id)
        {
            //GetActivityNameByActivityOwner
            var iCurrentYear = DateTime.Today.Year;
            List<ActivityType> olist = new List<ActivityType>();
            List<ActivityType> list = new List<ActivityType>();

            var query = repoActivityCode.GetAll().Result.Join(repoBudgetBook.GetAll().Result
               .Where(c => c.YYear == iCurrentYear), actCode => actCode.ID, butbook => butbook.ActivityCodeID, (actCode, butbook) => new { actCode, butbook })
               .Where(t => t.actCode.ActivityOwnerID == id)
               .GroupBy(t => t.actCode.ActivityOwnerID)
               .Select(group => group.First());

            foreach (var b in query)
            {
                //Source: https://stackoverflow.com/questions/22534035/lambda-expression-to-return-one-result-for-each-distinct-value-in-list
                //var list = callList.GroupBy(x => x.ApplicationID).Select(x => x.First()).ToList();
                var r = repoActivityName.GetAll().Result.Where(t => t.ID == b.butbook.ActivityTypeID)
                .Select(o => new ActivityType
                {
                    ID = o.ID,
                    ActivityName = o.ActivityName
                });

                olist.Add(r.FirstOrDefault());
            }

            list.AddRange(olist);

            list.Insert(0, new ActivityType { ID = 0, ActivityName = "Select Activity Name..." });
            return Json(new SelectList(list, "ID", "ActivityName"));
        }

        public JsonResult GetScopeByActivityOwner(long id)
        {
            //GetScopeByActivityOwner
            var iCurrentYear = DateTime.Today.Year;
            List<Scope> olist = new List<Scope>();
            List<Scope> list = new List<Scope>();
            //List<BudgetBook> bbook = repoBudgetBook.GetAll().Result.Where(c => c.ActivityOwnerID == id && c.YYear == iCurrentYear).GroupBy(x => x.ScopeID).Select(x => x.First()).ToList();

            var query = repoActivityCode.GetAll().Result.Join(repoBudgetBook.GetAll().Result
               .Where(c => c.YYear == iCurrentYear), actCode => actCode.ID, butbook => butbook.ActivityCodeID, (actCode, butbook) => new { actCode, butbook })
               .Where(t => t.actCode.ActivityOwnerID == id)
               .GroupBy(t => t.actCode.ActivityOwnerID)
               .Select(group => group.First());

            foreach (var b in query)
            {
                //Source: https://stackoverflow.com/questions/22534035/lambda-expression-to-return-one-result-for-each-distinct-value-in-list
                //var list = callList.GroupBy(x => x.ApplicationID).Select(x => x.First()).ToList();
                var r = repoScope.GetAll().Result.Where(t => t.ID == b.butbook.ScopeID)
                .Select(o => new Scope
                {
                    ID = o.ID,
                    Purpose = o.Purpose
                });

                olist.Add(r.FirstOrDefault());
            }

            list.AddRange(olist);

            list.Insert(0, new Scope { ID = 0, Purpose = "Select Scope..." });
            return Json(new SelectList(list, "ID", "Purpose"));
        }

        public JsonResult GetBudgetBasisByActivityOwner(long id)
        {
            //GetBudgetBasisByActivityOwner
            var iCurrentYear = DateTime.Today.Year;
            List<BudgetBasis> olist = new List<BudgetBasis>();
            List<BudgetBasis> list = new List<BudgetBasis>();
            //List<BudgetBook> bbook = repoBudgetBook.GetAll().Result.Where(c => c.ActivityOwnerID == id && c.YYear == iCurrentYear).GroupBy(x => x.BudgetBasisID).Select(x => x.First()).ToList();

            var query = repoActivityCode.GetAll().Result.Join(repoBudgetBook.GetAll().Result
              .Where(c => c.YYear == iCurrentYear), actCode => actCode.ID, butbook => butbook.ActivityCodeID, (actCode, butbook) => new { actCode, butbook })
              .Where(t => t.actCode.ActivityOwnerID == id)
              .GroupBy(t => t.actCode.ActivityOwnerID)
              .Select(group => group.First());

            foreach (var b in query)
            {
                //Source: https://stackoverflow.com/questions/22534035/lambda-expression-to-return-one-result-for-each-distinct-value-in-list
                //var list = callList.GroupBy(x => x.ApplicationID).Select(x => x.First()).ToList();
                var r = repoBudgetBasis.GetAll().Result.Where(t => t.ID == b.butbook.BudgetBasisID)
                .Select(o => new BudgetBasis
                {
                    ID = o.ID,
                    BudgetBase = o.BudgetBase
                });

                olist.Add(r.FirstOrDefault());
            }

            list.AddRange(olist);

            list.Insert(0, new BudgetBasis { ID = 0, BudgetBase = "Select Budget Base..." });
            return Json(new SelectList(list, "ID", "BudgetBase"));
        }

        #endregion

        #endregion

        #region  ==================== Budget Book Commitments Codes ====================
        public IEnumerable<BudgetBookCommitmentViewModel> GetBudgetBookCommitments(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentById(long? id)
        {
            try
            {
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.ID == id).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsByFocalPoint(long? id)
        {
            try
            {
                int? iYear = DateTime.Today.Year;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.FocalPointID == id && o.iYear == iYear).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsByAccountableManager(long? id)
        {
            try
            {
                int? iYear = DateTime.Today.Year;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                //var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.SponsorID == id && o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.iYear == iYear).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,


                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> CreateUpdateBBCommitment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CommitmentControlViewModel model = new CommitmentControlViewModel
            //{
            //    //oBudgetBook = GetBudgetBooks(iYear).Where(o => o.ID == id).FirstOrDefault(),
            //    oBudgetBook = GetBudgetBook(id).FirstOrDefault(),
            //};

            CommitmentControlViewModel model = new CommitmentControlViewModel();
            model.oBudgetBook = GetBudgetBook(id).Result.FirstOrDefault();
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(id); //.Where(o => o.BudgetBookID == id);

            ViewBag.vehicleID = new SelectList((repoVehicle.GetAll().Result != null) ? repoVehicle.GetAll().Result.OrderBy(o => o.VehicleName).ToList() : null, "ID", "VehicleName");
            ViewBag.groupID = new SelectList((repoPurchaseGroup.GetAll().Result != null) ? repoPurchaseGroup.GetAll().Result.OrderBy(o => o.GroupName).ToList() : null, "ID", "GroupName");
            ViewBag.teamID = new SelectList((repoTeam.GetAll().Result != null) ? repoTeam.GetAll().Result.OrderBy(o => o.TeamName).ToList() : null, "ID", "TeamName");
            ViewBag.typeID = new SelectList((repoPlannedEmmergency.GetAll().Result != null) ? repoPlannedEmmergency.GetAll().Result.OrderBy(o => o.PlanEmmerType).ToList() : null, "ID", "PlanEmmerType");
            ViewBag.statusID = new SelectList((repoStatus.GetAll().Result != null) ? repoStatus.GetAll().Result.OrderBy(o => o.ReqstStatus).ToList() : null, "ID", "ReqstStatus");

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_BudgetBookCommit", model);
        }

        public IActionResult EditBudgetBookCommitment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CommitmentControlViewModel model = new CommitmentControlViewModel
            //{
            //    //model.oCommitment = GetCommitments().FirstOrDefault(o => o.ID == id);
            //    oBudgetBookCommitment = GetBudgetBookCommitmentById(id).FirstOrDefault()
            //};

            CommitmentControlViewModel model = new CommitmentControlViewModel();
            model.oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault();
            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault(); //GetBudgetBooks(iYear).Where(o => o.ID == model.oBudgetBookCommitment.BudgetBookID).FirstOrDefault();

            ViewBag.vehicleID = new SelectList((repoVehicle.GetAll().Result != null) ? repoVehicle.GetAll().Result.OrderBy(o => o.VehicleName).ToList() : null, "ID", "VehicleName", model.oBudgetBookCommitment.vehicleID);
            ViewBag.groupID = new SelectList((repoPurchaseGroup.GetAll().Result != null) ? repoPurchaseGroup.GetAll().Result.OrderBy(o => o.GroupName).ToList() : null, "ID", "GroupName", model.oBudgetBookCommitment.groupID);
            ViewBag.teamID = new SelectList((repoTeam.GetAll().Result != null) ? repoTeam.GetAll().Result.OrderBy(o => o.TeamName).ToList() : null, "ID", "TeamName", model.oBudgetBookCommitment.teamID);
            ViewBag.typeID = new SelectList((repoPlannedEmmergency.GetAll().Result != null) ? repoPlannedEmmergency.GetAll().Result.OrderBy(o => o.PlanEmmerType).ToList() : null, "ID", "PlanEmmerType", model.oBudgetBookCommitment.typeID);
            ViewBag.statusID = new SelectList((repoStatus.GetAll().Result != null) ? repoStatus.GetAll().Result.OrderBy(o => o.ReqstStatus).ToList() : null, "ID", "ReqstStatus", model.oBudgetBookCommitment.statusID);


            //Cost Breakdown
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id);  //.Where(o => o.BudgetBookCommitmentsID == id);

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year && o.iDay == DateTime.Today.Day);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_AddCommitment", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdateBBCommitment(CommitmentControlViewModel model)
        {
            //Email Template Stuffs
            //Source: https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var webRoot = _env.WebRootPath; //get wwwroot Folder
            //Get TemplateFile located at wwwroot/ Templates / EmailTemplate / Register_EmailTemplate.html
            long? returnedId = 0;

            //if (ModelState.IsValid)
            //{
                string UserMail = Apps.getFullEmail(User.Identity.Name);
                AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
                
                if (LoginUser != null)
                {
                    var commitNo = CommitmentUniqueIdentifier.CommitmentIdentifier(repoBudgetBookCommitments).sCommitmentNumber;

                    bool isNew = !model.oBudgetBookCommitment.ID.HasValue;
                    BudgetBookCommitments entity = isNew ? new BudgetBookCommitments { AddedDate = DateTime.Today.Date, Comitmntno = commitNo } : await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);

                    entity.title = model.oBudgetBookCommitment.title;
                    entity.PONumber = model.oBudgetBookCommitment.PONumber;
                    entity.POValue = model.oBudgetBookCommitment.POValue;
                    entity.PRNumber = model.oBudgetBookCommitment.PRNumber;
                    entity.PRValue = model.oBudgetBookCommitment.PRValue;

                    entity.groupID = model.oBudgetBookCommitment.groupID;
                    entity.teamID = model.oBudgetBookCommitment.teamID;
                    entity.typeID = model.oBudgetBookCommitment.typeID;
                    entity.statusID = model.oBudgetBookCommitment.statusID;
                    entity.vehicleID = model.oBudgetBookCommitment.vehicleID;
                    entity.justification = model.oBudgetBookCommitment.justification;
                    entity.threat = model.oBudgetBookCommitment.threat;
                    entity.contractnovendor = model.oBudgetBookCommitment.contractnovendor;
                    entity.sPeriodfrom = model.oBudgetBookCommitment.sPeriodfrom;

                    entity.iYear = DateTime.Now.Year;
                    entity.ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.Draft; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("DRAFT")).FirstOrDefault().ID;
                    entity.ModifiedDate = DateTime.Now.Date;

                    entity.SponsorID = repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.AccountableManagerID;
                    entity.LineManagerID = repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.LineManagerID;
                    entity.ActivityOwnerID = repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.ActivityOwnerID;

                    if (isNew)
                    {
                        entity.BudgetBookID = model.oBudgetBook.ID;
                        entity.FocalPointID = LoginUser.ID;
                        returnedId = await repoBudgetBookCommitments.Insert(entity);
                        model.lstCostBreakdown = GetCostBreakdownByCommitmentId(returnedId);
                    }
                    else
                    {
                        //int iCurrentYear = DateTime.Today.Year;
                        //model.oBudgetBook = GetBudgetBook(entity.BudgetBookID).FirstOrDefault();

                        returnedId = model.oBudgetBookCommitment.ID;
                        await repoBudgetBookCommitments.Update(entity);
                    }
                }
            //}

            //Source: https://stackoverflow.com/questions/1257482/redirecttoaction-with-parameter
            return RedirectToAction("CostBreakdown", new { id = returnedId });
        }

        public JsonResult JsonGetBudgetBookCommitments(int? iYear)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            return Json(GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID));
            //return Json(GetBudgetBookCommitments(iYyear).Where(o => o.FocalPointID == LoginUser.ID));
        }

        //public JsonResult JsonExtGetBudgetBookCommitments(int? iYear)
        //{
        //    int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
        //    return Json(GetBudgetBookCommitments(iYyear));
        //}

        //[HttpGet]
        [HttpPost]
        public JsonResult GetCommitments(string query)
        {
            var oCommitments = (from o in repoBudgetBookCommitments.GetAll().Result
                                where o.title.StartsWith(query)
                                select new
                                {
                                    label = o.title,
                                    val = o.ID
                                }).ToList();

            //var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.title.ToLower().Contains(query));
            return Json(oCommitments.Take(10).ToList());
        }
   
        #endregion

        #region  ==================== Cost Breakdown Codes ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpdateCostBreakdown(CommitmentControlViewModel model, int? iYear)
        {
            //TODO: there are 2 hard coded lines here, find time to change them to programmer's code
            //if (ModelState.IsValid)
            //{

            if (model.CostBreakdown.CurrenciesID == null)
            {
                TempData["Message"] = "Your entry was not submitted, you did not select currency. Please select currency.";
            }
            else
            {
                bool isNew = !model.CostBreakdown.ID.HasValue;
                ActivityDetails entity = isNew ? new ActivityDetails { AddedDate = DateTime.Today.Date } : await repoCostBreakdown.GetById(model.CostBreakdown.ID);

                entity.BudgetBookCommitmentsID = model.oBudgetBookCommitment.ID;
                entity.Description = model.CostBreakdown.Description;
                entity.Quantity = model.CostBreakdown.Quantity;
                entity.Rate = model.CostBreakdown.Rate;
                entity.FixedExchangeRate = model.CostBreakdown.FixedExchangeRate;
                entity.CurrenciesID = model.CostBreakdown.CurrenciesID;
                entity.ModifiedDate = DateTime.Now.Date;
                entity.BudgetBookID = model.oBudgetBookCommitment.BudgetBookID;
                entity.iYear = DateTime.Now.Year;

                //TODO: Urgent Attention Check to see that the amount to be added into the database is less than the Budget
                var ExcResult = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
                var Exchange = (ExcResult.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : ExcResult.FirstOrDefault();

                //Get the list of all expenditure from the given budget line, and sum up (CurrentTotalExpendedFromBudgetLine) based on Currency type in the codes below.
                var lstAllExpendedFromBudgetLine = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID);
                decimal? SumTotalExpendedFromBudgetLine = 0.0M;
                foreach (var t in lstAllExpendedFromBudgetLine)
                {
                    SumTotalExpendedFromBudgetLine += (t.CurrencyName == "US Dollar")
                        ? (t.Rate * t.Quantity) : (t.FixedExchangeRate == 0.0M || t.FixedExchangeRate == null)
                        ? (t.Rate / Exchange.FloatingExchangeRate) * t.Quantity : (t.Rate / t.FixedExchangeRate) * t.Quantity;
                }

                var Currenci = repoCurrencies.GetAll().Result.ToList();
                var currenci = Currenci.First(c => model.CostBreakdown.CurrenciesID == c.ID);

                var ToBeSpent = (currenci.CurrencyName == "US Dollar")
                        ? (model.CostBreakdown.Rate * model.CostBreakdown.Quantity) : (model.CostBreakdown.FixedExchangeRate == 0.0M || model.CostBreakdown.FixedExchangeRate == null)
                        ? (model.CostBreakdown.Rate / Exchange.FloatingExchangeRate) * model.CostBreakdown.Quantity : (model.CostBreakdown.Rate / model.CostBreakdown.FixedExchangeRate) * model.CostBreakdown.Quantity;

                model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
                //var CurrentBalance = model.oBudgetBook.NAPIMSBUDGETFDollar - SumTotalExpendedFromBudgetLine;
                var CurrentBalance = model.oBudgetBook.NAPIMSBUDGETFDollar;

                if (ToBeSpent <= CurrentBalance)
                {
                    if (isNew) await repoCostBreakdown.Insert(entity);
                    else await repoCostBreakdown.Update(entity);
                }
                else
                {
                    var tCurrenci = repoCurrencies.GetAll().Result.Where(o => o.ID == model.CostBreakdown.CurrenciesID).FirstOrDefault();
                    if (tCurrenci.CurrencyName == "Naira")
                        TempData["Message"] = "Your Total Commitment N" + stringRoutine.formatAsBankMoney(ToBeSpent) + " is greater than the Amount $" + stringRoutine.formatAsBankMoney(CurrentBalance) + " left in the NAPIMS(F USD). \\nUpdate your cost breakdown to accommodate your expenditure.";
                    else
                        TempData["Message"] = "Your Total Commitment $" + stringRoutine.formatAsBankMoney(ToBeSpent) + " is greater than the Amount $" + stringRoutine.formatAsBankMoney(CurrentBalance) + " left in the NAPIMS(F USD). \\nUpdate your cost breakdown to accommodate your expenditure.";
                }
            }

            //Source: https://stackoverflow.com/questions/1257482/redirecttoaction-with-parameter
            return RedirectToAction("CostBreakdown", new { id = model.oBudgetBookCommitment.ID });
        }

        //GET: Commitments/EditCostBreakdown/5 (Get Specific Cost Breakdown Item by its Id)
        public IActionResult EditCostBreakdown(long? id, int? iYear)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                //CostBreakdown = GetCostBreakdown().FirstOrDefault(o => o.ID == id)
                CostBreakdown = GetCostBreakdownById(id).FirstOrDefault()
            };
            model.oBudgetBookCommitment = GetBudgetBookCommitmentById(model.CostBreakdown.BudgetBookCommitmentsID).Result.FirstOrDefault(); //.Where(o => o.ID == model.CostBreakdown.BudgetBookCommitmentsID).FirstOrDefault();

            //ViewBag.Currency = new SelectList(repoCurrencies.GetAll().Result, "ID", "CurrencyName", model.CostBreakdown.CurrenciesID);
            model.lstCurrencies = repoCurrencies.GetAll().Result.ToList();
            ViewBag.Currency = model.lstCurrencies;
            //model.CostBreakdown.CurrenciesID = "";


            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_CostBreakdownForm", model);
        }

        // POST: Commitments/DeleteCostBreakdownConfirmed/5
        [HttpPost]
        public async Task<IActionResult> DeleteCostBreakdown(long? id)
        {
            var entity = await repoCostBreakdown.GetById(id);
            if (entity != null)
            {
                await repoCostBreakdown.Delete(entity);
            }

            return RedirectToAction("CostBreakdown", new { id = entity.BudgetBookCommitmentsID });
        }

        #endregion

        #region ==================== For Focal Point View ====================

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsPending(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Draft && o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsSentForApproval(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview 
                                                                        || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                                        || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession)
                                                                        && o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsApproved(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented 
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications 
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) && o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsRejected(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.iYear == iYear && (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown
                                                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined
                                                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered
                                                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed)).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> LoadPendingBudgetCommitments(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

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

                var customerData = Enumerable.Empty<BudgetBookCommitmentViewModel>();
                if (LoginUser.RoleId == (int)enuRole.Admin)
                {
                    customerData = await GetBudgetBookCommitmentsPending(iYyear);
                }
                else
                {
                    customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Draft && o.iYear == iYyear);
                }

                //getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadBudgetCommitmentsSentForApproval(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

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

                //var customerData = GetFocalPointData(LoginUser).Where(o => (o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD") || o.ApprovalStatus.Contains("CCPANEL")) && o.iYear == iYyear);  // getting all Customer data
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => (o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD") || o.ApprovalStatus.Contains("CCPANEL")) && o.iYear == iYyear);
                var customerData = Enumerable.Empty<BudgetBookCommitmentViewModel>();
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => o.ApprovalID == (long)enuApproval.LineManagerReview || o.ApprovalID == (long)enuApproval.TBD || o.ApprovalID == (long)enuApproval.PendingCCPanelSession && o.iYear == iYyear); // && o.iYear == iYyear
                if (LoginUser.RoleId == (int)enuRole.Admin)
                {
                    //customerData = GetBudgetBookCommitments().Where(o => (o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD") || o.ApprovalStatus.Contains("CCPANEL")) && o.iYear == iYyear);
                    customerData = await GetBudgetBookCommitmentsSentForApproval(iYyear); //.ToList().Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession); // && o.iYear == iYyear
                }
                else
                {
                    customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession && o.iYear == iYyear);
                }

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadApprovedBudgetCommitments(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

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

                //var customerData = GetFocalPointData(LoginUser).Where(o => o.ApprovalStatus.Contains("APPRO") && o.FocalPointID == LoginUser.ID);  // getting all Customer data
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => o.ApprovalStatus.Contains("APPRO") && o.FocalPointID == LoginUser.ID);
                var customerData = Enumerable.Empty<BudgetBookCommitmentViewModel>();
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => o.ApprovalID == (long)enuApproval.ApprovedAsPresented || o.ApprovalID == (long)enuApproval.ApprovedWithModification || o.ApprovalID == (long)enuApproval.ProvisionalApproval && o.iYear == iYyear); // && o.iYear == iYyear
                if (LoginUser.RoleId == (int)enuRole.Admin)
                {
                    //customerData = GetBudgetBookCommitments().Where(o => o.ApprovalStatus.Contains("APPRO") && o.iYear == iYyear);
                    customerData = await GetBudgetBookCommitmentsApproved(iYyear);//.ToList().Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval); // && o.iYear == iYyear
                }
                else
                {
                    customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval && o.iYear == iYyear);
                }
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> LoadRejectedBudgetCommitments(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

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

                //var customerData = GetFocalPointData().Where(o => o.ApprovalStatus.Contains("STEPP") && o.ApprovalStatus.Contains("DECL") && o.ApprovalStatus.Contains("DEFFER") && o.ApprovalStatus.Contains("POST") && !o.ApprovalStatus.Contains("CCPANEL") && o.FocalPointID == LoginUser.ID);  // getting all Customer data
                //var customerData = GetFocalPointData(LoginUser).Where(o => (o.ApprovalStatus.Contains("STEPP") || o.ApprovalStatus.Contains("DECLI") || o.ApprovalStatus.Contains("DEFFER") || o.ApprovalStatus.Contains("POST")) && o.FocalPointID == LoginUser.ID && o.iYear == iYyear);  // getting all Customer data
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => (o.ApprovalStatus.Contains("STEPP") || o.ApprovalStatus.Contains("DECLI") || o.ApprovalStatus.Contains("DEFFER") || o.ApprovalStatus.Contains("POST")) && o.FocalPointID == LoginUser.ID && o.iYear == iYyear);  // getting all Customer data
                var customerData = Enumerable.Empty<BudgetBookCommitmentViewModel>();
                //var customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).ToList().Where(o => o.ApprovalID == (long)enuApproval.SteppedDown || o.ApprovalID == (long)enuApproval.Declined || o.ApprovalID == (long)enuApproval.DefferedFurtherDocumentationRequired || o.ApprovalID == (long)enuApproval.PostPoned && o.iYear == iYyear); // && o.iYear == iYyear

                if (LoginUser.RoleId == (int)enuRole.Admin)
                {
                    //customerData = GetBudgetBookCommitments().Where(o => (o.ApprovalStatus.Contains("STEPP") || o.ApprovalStatus.Contains("DECL") || o.ApprovalStatus.Contains("DEFFER") || o.ApprovalStatus.Contains("POST")) && o.iYear == iYyear);
                    customerData = await GetBudgetBookCommitmentsRejected(iYyear); // && o.iYear == iYyear
                }
                else
                {
                    customerData = GetBudgetBookCommitmentsByFocalPoint(LoginUser.ID).Result.ToList().Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown 
                                                                                                                    || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined 
                                                                                                                    || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered 
                                                                                                                    || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed && o.iYear == iYyear);
                }
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ==================== For Activity Owner View ====================

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsByActivityOwner(long? id)
        {
            try
            {
                int? iYear = DateTime.Today.Year;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.ActivityOwnerID == id && o.iYear == iYear).ToList().Select(entity =>
                {
                    //var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetActivityOwnerData()
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            return await GetBudgetBookCommitmentsByActivityOwner(LoginUser.ID);
        }

        public async Task<IActionResult> LoadOwnerPendingBudgetCommitments()
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

                //var customerData = GetActivityOwnerData().Where(o => o.ApprovalStatus.Contains("TBD"));
                var customerData = GetActivityOwnerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD);

                // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadOwnerBudgetCommitmentsSentForApproval()
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

                //var customerData = GetActivityOwnerData().Where(o => (o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD") || o.ApprovalStatus.Contains("CCPANEL")));  // getting all Customer data
                var customerData = GetActivityOwnerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadOwnerApprovedBudgetCommitments()
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

                //var customerData = GetActivityOwnerData().Where(o => o.ApprovalStatus.Contains("APPRO"));  // getting all Customer data
                var customerData = GetActivityOwnerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> LoadOwnerRejectedBudgetCommitments()
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

                //var customerData = GetActivityOwnerData().Where(o => !o.ApprovalStatus.Contains("TBD") 
                //                                                && !o.ApprovalStatus.Contains("APPRO") 
                //                                                && !o.ApprovalStatus.Contains("DRAFT") 
                //                                                && !o.ApprovalStatus.Contains("MANAGER")
                //                                                && !o.ApprovalStatus.Contains("CCPANEL"));

                var customerData = GetActivityOwnerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined
                                                               || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered
                                                               || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed
                                                               || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ==================== For Line Manager View ====================

        /*Line Manager should only see Commitment Control Request from the 
        Budget lines that belong to Activity Code of which he/she is a Line Manager*/

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsByLineManager(long? id)
        {
            try
            {
                int? iYear = DateTime.Today.Year;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.LineManagerID == id && o.iYear == iYear).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Line Manager
        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetLineManagerData()
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            
            var data = GetBudgetBookCommitmentsByLineManager(LoginUser.ID).Result.ToList();
            return data;
        }

        public async Task<IActionResult> LoadLMPendingCommitments()
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

                //var customerData = GetLineManagerData().Where(o => o.ApprovalStatus.Contains("MANAGER"));  // getting all Customer data
                var customerData = GetLineManagerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadLMApprovedCommitments()
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
                
                //var customerData = GetLineManagerData().Where(o => o.ApprovalStatus.Contains("APPRO"));  // getting all Customer data
                var customerData = GetLineManagerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented 
                                                                || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications 
                                                                || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> LoadLMRejectedCommitments()
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
                //var customerData = GetLineManagerData().Where(o => !o.ApprovalStatus.Contains("TBD") && !o.ApprovalStatus.Contains("APPRO") 
                //                                                && !o.ApprovalStatus.Contains("DRAFT") && !o.ApprovalStatus.Contains("MANAGER") 
                //                                                && !o.ApprovalStatus.Contains("CCPANEL"));
                var customerData = GetLineManagerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined
                                                                || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered
                                                                || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed
                                                                || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> LoadLMCommitmentsSentForApproval()
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
                //var customerData = GetLineManagerData().Where(o => o.ApprovalStatus.Contains("CCPANEL")); // getting all Customer data
                var customerData = GetLineManagerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ==================== Reviewer/Approver Section ====================

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetAccountableManagerData()
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            return GetBudgetBookCommitmentsByAccountableManager(LoginUser.ID).Result.ToList();
        }

        public async Task<IActionResult> LoadActionsPendingMyReview()
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

                //var customerData = GetAccountableManagerData().Where(o => o.ApprovalStatus.Contains("MANAGER"));  // getting all Customer data
                //var customerData = GetAccountableManagerData().Where(o => o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD"));
                var customerData = GetAccountableManagerData().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview
                                                                       || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD);

                // && !o.ApprovalStatus.Contains("APPRO")
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadActionsIApproved()
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

                //var customerData = GetAccountableManagerData().Where(o => o.ApprovalStatus.Contains("APPRO"));  // getting all Customer data
                var customerData = GetAccountableManagerData().Result.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadActionsIRejected()
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

                var customerData = GetAccountableManagerData().Result.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications 
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview
                                                            && o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ==================== For CCP Session View ====================

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetCCPSessionBudgetBookCommitments(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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


                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.iYear == iYyear).ToList().Select(entity =>
                {
                    //var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID); //0.0M;

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,


                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                    //}
                }).ToList().OrderByDescending(o => o.ID);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<BudgetBookCommitmentViewModel> GetCCPSessionBudgetBookCommitmentsPending(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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


                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.iYear == iYyear && o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession).ToList().Select(entity =>
                {
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID); //0.0M;

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,


                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                    //}
                }).ToList().OrderByDescending(o => o.ID);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetCCPSessionBudgetBookCommitmentsApproved(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                //var ApprovalStatuses = repoApprovalDecision.GetAll().Result.ToList();
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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


                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)
                                                                         && o.iYear == iYyear).ToList().Select(entity =>
                {
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID); //0.0M;

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,


                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                    //}
                }).ToList().OrderByDescending(o => o.ID);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetCCPSessionBudgetBookCommitmentsRejected(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                //string UserMail = Apps.getFullEmail(User.Identity.Name);
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined 
                                                                        || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered
                                                                        || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown 
                                                                        || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed
                                                                        && o.iYear == iYyear).ToList().Select(entity =>
                {
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID); //0.0M;

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        //sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,

                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,


                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                    //}
                }).ToList().OrderByDescending(o => o.ID);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> LoadActionsPendingCCPSession(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

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

                var customerData = new List<BudgetBookCommitmentViewModel>();
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

                //var ot = GetCCPSessionBudgetBookCommitments(iYyear).Where(o => o.ApprovalStatus.Contains("CCPANEL"));
                var ot = GetCCPSessionBudgetBookCommitmentsPending(iYyear);
                customerData = ot.ToList();

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadActionsApprovedByCCPSession(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

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

                var customerData = new List<BudgetBookCommitmentViewModel>();
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

                //var ot = GetCCPSessionBudgetBookCommitments(iYyear).Where(o => o.ApprovalStatus.Contains("APPRO"));
                var ot = await GetCCPSessionBudgetBookCommitmentsApproved(iYyear);

                foreach (var oItem in ot)
                {
                    var totSum = GetTotalSumCostBreakDownByCommitmentId(oItem.ID);
                    //var totSum = GetCostBreakdown(oItem.ID).Select(o => o.Quantity * o.Rate).Sum();
                    if (totSum >= appSettings.Value.Threshold)
                    {
                        customerData.Add(oItem);
                    }
                }

                //var customerData = CCPSesionItems.Where(o => o.ApprovalStatus.Contains("APPRO"));  // getting all Customer data

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> LoadActionsRejectedByCCPSession(int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

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

                var customerData = new List<BudgetBookCommitmentViewModel>();
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

                //var ot = GetCCPSessionBudgetBookCommitments(iYyear).Where(o => !o.ApprovalStatus.Contains("TBD") && !o.ApprovalStatus.Contains("APPRO")
                //                                                            && !o.ApprovalStatus.Contains("DRAFT") && !o.ApprovalStatus.Contains("MANAGER")
                //                                                            && !o.ApprovalStatus.Contains("CCPANEL"));

                var ot = await GetCCPSessionBudgetBookCommitmentsRejected(iYyear); //.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Declined && o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Deffered
                                                                             //&& o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.SteppedDown && o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Postponed);

                foreach (var oItem in ot)
                {
                    var totSum = GetTotalSumCostBreakDownByCommitmentId(oItem.ID);
                    //var totSum = GetCostBreakdown(oItem.ID).Select(o => o.Quantity * o.Rate).Sum();
                    if (totSum >= appSettings.Value.Threshold)
                    {
                        customerData.Add(oItem);
                    }
                }

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IActionResult LoadActionsApprovedByLineManager(int? iYear)
        //{
        //    string UserMail = Apps.getFullEmail(User.Identity.Name);
        //    AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

        //    try
        //    {
        //        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault(); // Skip number of Rows count  
        //        var length = Request.Form["length"].FirstOrDefault(); // Paging Length 10,20  
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(); // Sort Column Name  
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(); // Sort Column Direction (asc, desc)  
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault().ToUpper(); // Search Value from (Search box)  
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0; //Paging Size (10, 20, 50,100)  
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;

        //        var customerData = new List<BudgetBookCommitmentViewModel>();
        //        int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

        //        //var ot = GetCCPSessionBudgetBookCommitments(iYyear).Where(o => o.ApprovalStatus.Contains("APPRO") && repoUsers.GetById(o.ApproverID).RoleId != (int)enuRole.Admin);

        //        var ot = GetCCPSessionBudgetBookCommitments(iYyear).Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
        //                                                                    || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
        //                                                                    || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) && repoUsers.GetById(o.ApproverID).RoleId != (int)enuRole.Admin);

        //        foreach (var oItem in ot)
        //        {
        //            var totSum = GetTotalSumCostBreakDownByCommitmentId(oItem.ID);
        //            //var totSum = GetCostBreakdown(oItem.ID).Select(o => o.Quantity * o.Rate).Sum();
        //            if (totSum >= appSettings.Value.Threshold)
        //            {
        //                customerData.Add(oItem);
        //            }
        //        }

        //        //var customerData = CCPSesionItems.Where(o => o.ApprovalStatus.Contains("APPRO"));  // getting all Customer data

        //        //Sorting  
        //        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //        {
        //            //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
        //        }
        //        //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.title.Contains(searchValue)); //Search  

        //        recordsTotal = customerData.Count(); //total number of rows counts   
        //        var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #region  ==================== Commitment Presentation and Approval ==================== 

        // GET: Commitments/ApprovedCommitmentDetails/5
        public async Task<IActionResult> ApprovedCommitmentDetails(long? id, int? iYear)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault() //.Where(o => o.ID == id).FirstOrDefault()
            };
            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id); //.Where(o => o.BudgetBookCommitmentsID == id);
            model.LstUploadFiles = await GetUploadedFiles(id);
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID); //.Where(o => o.BudgetBookID == model.oBudgetBookCommitment.BudgetBookID);

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year && o.iDay == DateTime.Today.Day);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            model.lstCurrencies = repoCurrencies.GetAll().Result.ToList();
            ViewBag.Currency = model.lstCurrencies;

            if (model == null)
            {
                return NotFound();
            }

            //ViewData["approverID"] = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Approver), "ID", "FullName", model.oBudgetBookCommitment.ApproverID);
            //ViewData["approvalID"] = new SelectList(repoApprovalDecision.GetAll().Result, "ID", "Decision", model.oBudgetBookCommitment.ApprovalID);

            return PartialView("_ApprovedCommitmentControlView", model);
        }


        public async Task<IActionResult> ViewCommitmentDetails(long? id, int? iYear)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault() //.Where(o => o.ID == id).FirstOrDefault()
            };
            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id); //.Where(o => o.BudgetBookCommitmentsID == id);
            model.LstUploadFiles = await GetUploadedFiles(id);
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID); //.Where(o => o.BudgetBookID == model.oBudgetBookCommitment.BudgetBookID);

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year && o.iDay == DateTime.Today.Day);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            model.lstCurrencies = repoCurrencies.GetAll().Result.ToList();
            ViewBag.Currency = model.lstCurrencies;

            if (model == null)
            {
                return NotFound();
            }

            ViewData["approverID"] = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Corporate), "ID", "FullName", model.oBudgetBookCommitment.ApproverID);
            //TODO: ViewData["approvalID"] = new SelectList(repoApprovalDecision.GetAll().Result, "ID", "Decision", model.oBudgetBookCommitment.ApprovalID);

            return PartialView("_ApprovedCommitmentControlView", model);
        }

        #endregion

        #region  ==================== Upload and Download Files  ====================

        private async Task<IEnumerable<UploadFilesViewModel>> GetUploadedFiles(long? id)
        {
            try
            {
                var result = repoFileUpload.GetAll().Result.Where(o => o.BudgetBookCommitmentsID == id).ToList().Select(entity =>
                {
                    return new UploadFilesViewModel
                    {
                        ID = entity.ID,
                        Title = entity.Title,
                        UploadDT = entity.AddedDate,
                        UploadFilesSize = entity.UploadFiles.Length,
                        BudgetBookCommitmentsID = entity.BudgetBookCommitmentsID,
                        FileNames = entity.FileNames,
                        //CommitmentID = entity.CommitmentID
                    };
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> UploadFiles(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel
            {
                oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault(), //GetBudgetBookCommitments(iYear).Where(o => o.ID == id).FirstOrDefault(),
                LstUploadFiles = await GetUploadedFiles(id)
            };

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_Attachments", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUploadedFile(long? id)
        {
            var entity = await repoFileUpload.GetById(id);

            if (entity != null)
            {
                await repoFileUpload.Delete(entity);
            }

            return RedirectToAction("CostBreakdown", new { id = entity.BudgetBookCommitmentsID });
            //return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> UploadFilesT(CommitmentControlViewModel model)
        {
            try
            {                
                FileUpload entity = new FileUpload
                {
                    BudgetBookCommitmentsID = model.oBudgetBookCommitment.ID,
                    //entity.CommitmentID = model.oCommitment.ID;
                    Title = model.FileUpload.Title,
                    AddedDate = DateTime.Today.Date,
                    ModifiedDate = DateTime.Today.Date
                };

                if (model.FileUpload.UploadFiles == null || model.FileUpload.UploadFiles.Length == 0)
                {
                    TempData["Message"] = "No file selected";
                    //ViewBag.Message = "No file selected";
                    return RedirectToAction("CostBreakdown", new { id = entity.BudgetBookCommitmentsID });
                }

                if (model.FileUpload.Title == "")
                {
                    TempData["Message"] = "Please enter title for your attachment";
                    //ViewBag.Message = "Please enter title for your attachment";
                    return RedirectToAction("CostBreakdown", new { id = entity.BudgetBookCommitmentsID });
                }

                using (var memoryStream = new MemoryStream())
                {
                    await model.FileUpload.UploadFiles.CopyToAsync(memoryStream);
                    entity.UploadFiles = memoryStream.ToArray();
                    entity.FileNames = Path.GetFileName(model.FileUpload.UploadFiles.FileName);
                }
                await repoFileUpload.Insert(entity);
                //}

                //model.oCommitment = GetCommitments().FirstOrDefault(o => o.ID == id);
                //model.oBudgetBookCommitment = GetBudgetBookCommitments().Where(o => o.ID == model. id).FirstOrDefault();
                model.LstUploadFiles = await GetUploadedFiles(model.oBudgetBookCommitment.ID);

                return RedirectToAction("CostBreakdown", new { id = entity.BudgetBookCommitmentsID });
                //return PartialView("_Attachments", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("download/{id}")]
        [HttpGet]
        public async Task<FileStreamResult> Download(long? id)
        {
            try
            {
                var downloaded = await repoFileUpload.GetById(id);

                var content = new MemoryStream(downloaded.UploadFiles);
                var contentType = GetContentType(downloaded.FileNames);
                var fileName = downloaded.FileNames;
                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetContentType(string path)
        {
            try
            {
                var types = FileHelpers.GetMimeTypes();
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return types[ext];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  ==================== CLONING COMMITMENT CONTROL ====================

        public async Task<IActionResult> CloneBudgetBookCommitment(long? id)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            long? returnedId = 0;

            if (LoginUser != null)
            {
                var commitNo = CommitmentUniqueIdentifier.CommitmentIdentifier(repoBudgetBookCommitments).sCommitmentNumber;
                var oClone = await repoBudgetBookCommitments.GetById(id);
                var lstCostBreakdown = GetCostBreakdownByCommitmentId(id); //.Where(o => o.BudgetBookCommitmentsID == id);

                BudgetBookCommitments entity = new BudgetBookCommitments
                {
                    Comitmntno = commitNo,
                    title = "[Cloned from " + oClone.Comitmntno + "]" + oClone.title,
                    BudgetBookID = oClone.BudgetBookID,
                    CCPSessionDate = oClone.CCPSessionDate,
                    iYear = DateTime.Now.Year,
                    ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.Draft, //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("DRAFT")).FirstOrDefault().ID,
                    AddedDate = DateTime.Today.Date,
                    ModifiedDate = DateTime.Now.Date,
                    FocalPointID = LoginUser.ID,
                    ActivityOwnerID = oClone.ActivityOwnerID,
                    LineManagerID = oClone.LineManagerID,
                    SponsorID = oClone.SponsorID
                };

                returnedId = await repoBudgetBookCommitments.Insert(entity);

                //  Load all the cost breakdown

                foreach (ActivityDetailsViewModel t in lstCostBreakdown)
                {
                    ActivityDetails dtlEntity = new ActivityDetails
                    {
                        BudgetBookCommitmentsID = returnedId,
                        BudgetBookID = t.BudgetBookID,
                        Description = t.Description,
                        Quantity = t.Quantity,
                        Rate = t.Rate,
                        CurrenciesID = t.CurrenciesID,
                        FixedExchangeRate = t.FixedExchangeRate,
                        iYear = DateTime.Now.Year,
                        AddedDate = DateTime.Today.Date,
                        ModifiedDate = DateTime.Now.Date,
                    };

                    await repoCostBreakdown.Insert(dtlEntity);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloneCostBreakdown(CommitmentControlViewModel model)
        {
            if (ModelState.IsValid)
            {
                ActivityDetails entity = new ActivityDetails
                {
                    //entity.CommitmentID = model.oCommitment.ID;
                    BudgetBookCommitmentsID = model.oBudgetBookCommitment.ID,
                    BudgetBookID = model.CostBreakdown.BudgetBookID,
                    Description = model.CostBreakdown.Description,
                    Quantity = model.CostBreakdown.Quantity,
                    Rate = model.CostBreakdown.Rate,
                    CurrenciesID = model.CostBreakdown.CurrenciesID,
                    FixedExchangeRate = model.CostBreakdown.FixedExchangeRate,
                    iYear = DateTime.Now.Year,
                    AddedDate = DateTime.Today.Date,
                    ModifiedDate = DateTime.Now.Date
                };

                await repoCostBreakdown.Insert(entity);
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloneAttachments(CommitmentControlViewModel model)
        {
            if (ModelState.IsValid)
            {
                ActivityDetails entity = new ActivityDetails
                {
                    //entity.CommitmentID = model.oCommitment.ID;
                    BudgetBookCommitmentsID = model.oBudgetBookCommitment.ID,
                    Description = model.CostBreakdown.Description,
                    Quantity = model.CostBreakdown.Quantity,
                    Rate = model.CostBreakdown.Rate,
                    AddedDate = DateTime.Today.Date,
                    ModifiedDate = DateTime.Now.Date
                };

                await repoCostBreakdown.Insert(entity);
            }
            return NoContent();
        }

        #endregion

        #region  ==================== Record Deleting Codes but not curretly in use  ====================

        public async Task<IActionResult> DeleteDraftCommitment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await repoBudgetBookCommitments.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                //Delete Cost breakdowns if exists
                var entityCostBreakDown = repoCostBreakdown.GetAll().Result.Where(o => o.BudgetBookCommitmentsID == entity.ID);
                await repoCostBreakdown.DeleteRange(entityCostBreakDown);
                
                //Delete all documents if exist
                var entityDocuments = repoFileUpload.GetAll().Result.Where(o => o.BudgetBookCommitmentsID == entity.ID);
                await repoFileUpload.DeleteRange(entityDocuments);

                //Then Delete the Draft Commitment
                await repoBudgetBookCommitments.Delete(entity);
            }

            return View(entity);
        }
        #endregion

        #region  ==================== Work Flow procedures with Send Mails ====================

        //====================================================================================================
        //= Note: The four actions here are called by JavaScript onclick event located in a partial view on, =
        //= Commitments/CostBreakdown (_CostBreakdownList) partial view                                      =
        //= These are used to route the requests through application WorkFlow                                =
        //====================================================================================================

        //TODO: Work Flow Step 1 : Focal Point Sends for Activity Owner's Review. This is called from JavaScript on _CostBreakdownList.cshtml partial view
        public async Task<IActionResult> SendForActivityOwnerReview(long? id)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            //if (LoginUser != null)
            //{
            BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(id);

            decimal? TotalCostBreakdown = GetTotalSumCostBreakDownByCommitmentId(id);
            //if (entity.PRValue != TotalCostBreakdown)
            if (Math.Round(entity.PRValue, 2) != Math.Round(GetTotalSumCostBreakDownByCommitmentId(id), 2))
            {
                var message = "Dear " + LoginUser.FullName + ", please note that your PR Value and total commitment breakdown are not equal. \nThe system will not allow you to forward this request to Activity Owner until the values are equal.";
                TempData["Message"] = message;

                return Json(new { result = "Redirect", url = Url.Action("CostBreakdown", "Commitments", new { Id = entity.ID }) });
            }

            IEnumerable<ActivityDetails> entityDetails = repoCostBreakdown.GetAll().Result.Where(o => o.BudgetBookCommitmentsID == id);
            if (entityDetails.Count() > 0)
            {
                var BudGetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();
                entity.ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.TBD; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("TBD")).FirstOrDefault().ID;
                entity.ModifiedDate = DateTime.Now.Date;

                List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    AppUsers ActivityOwner = await repoUsers.GetById(BudGetBook.ActivityOwnerID);

                    IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                    //decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(id);
                    var finalValue = stringRoutine.formatAsBankMoney("$", TotalCostBreakdown);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));
                    mailTo.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));
                    //mailCopy.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));

                    //TODO: Send mail to Activity Owner, using MailKit
                    var FocalPointToActivityOwner = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "FocalPointToActivityOwner.html";

                    structUserMailIdx mailFrom = new structUserMailIdx
                    {
                        m_sUserMail = LoginUser.UserMail,
                        m_sUserName = LoginUser.FullName
                    };

                    var subject = entity.Comitmntno + " is awaiting your review.";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(FocalPointToActivityOwner))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                        BudGetBook.ActivityOwner,
                        entity.FocalPoint.FullName,
                        entity.title,
                        entity.Comitmntno,
                        BudGetBook.CapexOpex,
                        finalValue,
                        BudGetBook.ActivityCode,
                        BudGetBook.CostObject,
                        BudGetBook.ActivityType,
                        entity.FocalPoint.FullName,
                        repoUsers.GetById(BudGetBook.ActivityOwnerID).Result.FullName,
                        repoActivityCode.GetById(BudGetBook.ActivityCodeID).Result.LineManager.FullName,
                        repoUsers.GetById(BudGetBook.SponsorID).Result.FullName,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        GetBaseUrl()
                        );
                    await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                    TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully sent for Activity Owner's review.";
                }
            }
            else
            {
                //This is to ensure users do cost breakdown on every entry before sending to the Activity Owner for review.
                //Some users were found entering PR Value without cost breakdown, this is not allowed.
                //The cost breakdown and the PR Value must be equal. This is not enforced because, at the point of entry, 
                //some users do not have the actual PR value.
                TempData["Message"] = "Please kindly enter enter cost breakdown for your commitment. And ensure it is the same as your PR Value.";
            }

            //}
            //else
            //{
            //    TempData["Message"] = "Your session has expired. Kindly relogin into Commitment control";
            //}
            return Json(new { result = "Redirect", url = Url.Action("Index", "Commitments") });
            //return RedirectToAction("Index");
        }

        //TODO: Work Flow Step 2 : Activity Owner Sends for Line Manager's approval/review. This is called from JavaScript on _CostBreakdownList.cshtml partial view
        public async Task<IActionResult> SendForLineManagerApproval(long? id)
        {
            //TODO: When sent for approval by Activity Owner, the UserId of the Activity Owner is entered into the table 
            //so that the Line Manager will not be seeing the item in the Pending in-tray.
            //So, in the LoadLMPendingCommitments action, check if the ApproverID is null and when not null.
            //When null, the item should show in the in-tray, when not null it means it has been sent for approval.

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(id);
            decimal? TotalCostBreakdown = GetTotalSumCostBreakDownByCommitmentId(id);
            
            if (Math.Round(entity.PRValue, 2) != Math.Round(GetTotalSumCostBreakDownByCommitmentId(id), 2))
            {
                var message = "Dear " + LoginUser.FullName + ", please note that your PR Value and total commitment breakdown are not equal. The system will not allow you to forward this request to Line Manager, until the values are equal.";
                TempData["Message"] = message;

                return Json(new { result = "Redirect", url = Url.Action("CostBreakdown", "Commitments", new { Id = entity.ID }) });
                //return RedirectToAction("CostBreakdown", new { Id = entity.ID });
            }

            var BudGetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();
            if (LoginUser != null)
            {
                entity.ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("MANAGER")).FirstOrDefault().ID;
                entity.ModifiedDate = DateTime.Now.Date;

                List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    AppUsers LineManager = await repoUsers.GetById(BudGetBook.LineManagerID);
                    AppUsers ActivityOwner = await repoUsers.GetById(BudGetBook.ActivityOwnerID);
                    AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);
                    //Note: the Admins are the CCP Panel group
                    IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));

                    decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(id);
                    var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                    if (CurentTotalExpendedFromBudgetLine >= appSettings.Value.Threshold)
                    {
                        foreach (var admin in Admins)
                        {
                            mailTo.Add(new structUserMailIdx(admin.FullName, admin.UserMail, admin.ID.ToString()));
                        }
                    }

                    mailTo.Add(new structUserMailIdx(LineManager.FullName, LineManager.UserMail, LineManager.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));

                    //TODO: Send mail to Line Manager, copy Activity Owner using MailKit
                    var ActivityOwnerToLineManager = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "ActivityOwnerToLineManager.html";

                    ////TODO: Send mail to Sponsor/Accountable Manager, using MailKit
                    //var LineManegerToAccountableManager = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "LineManegerToAccountableManager.html";

                    structUserMailIdx mailFrom = new structUserMailIdx
                    {
                        m_sUserMail = LoginUser.UserMail,
                        m_sUserName = LoginUser.FullName
                    };

                    var subject = entity.Comitmntno + " is awaiting your review / approval.";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(ActivityOwnerToLineManager))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                        LineManager.FullName,
                        LoginUser.FullName,
                        entity.title,
                        entity.Comitmntno,
                        BudGetBook.CapexOpex,
                        finalValue,
                        BudGetBook.ActivityCode,
                        BudGetBook.CostObject,
                        BudGetBook.ActivityType,
                        LoginUser.FullName,
                        LineManager.FullName,
                        BudGetBook.ActivityOwner,
                        BudGetBook.Sponsor,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        GetBaseUrl()
                        );

                    await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                    TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully sent for Line Manager's approval.";
                }
            }
            else
            {
                TempData["Message"] = "Your session has expired. Kindly relogin into Commitment control";
            }

            var wkflowType = repoActivityCodeWorkStream.GetById(repoActivityCode.GetById(BudGetBook.ActivityCodeID).Result.ActivityCodeWorkStreamID).Result.WorkFlowType;

            if (wkflowType == (int)WorkFlowTypes.enuWorkFlowType.FP_LM_CCP)
            {
                return Json(new { result = "Redirect", url = Url.Action("Index", "Commitments") });
            }

            return Json(new { result = "Redirect", url = Url.Action("ActivityOwner", "Commitments") });
            //return RedirectToAction("ActivityOwner");
        }

        public async Task<IActionResult> ActivityOwnerDeclined(CommitmentControlViewModel model)
        {
            //Email Template Stuffs
            //Source: https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var webRoot = _env.WebRootPath; //get wwwroot Folder
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            if (LoginUser != null)
            {
                BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);
                int iCurrentYear = DateTime.Today.Year;
                model.oBudgetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();
                entity.ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.Declined; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("DECLI")).FirstOrDefault().ID; //model.oBudgetBookCommitment.ApprovalID;
                entity.ApprovalComment = model.oBudgetBookCommitment.ApprovalComment;
                entity.Savings = model.oBudgetBookCommitment.Savings;
                entity.ModifiedDate = DateTime.Today.Date;

                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                    List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                    AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);

                    //TODO: Activity Owner to Send mail to Focal Point, using MailKit
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));
                    mailTo.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));

                    var ActivityOwnerToFocalPointDeclined = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "ActivityOwnerToFocalPointDeclined.html";

                    structUserMailIdx mailFrom = new structUserMailIdx
                    {
                        m_sUserMail = LoginUser.UserMail,
                        m_sUserName = LoginUser.FullName
                    };

                    //var ApprovalDecision = repoApprovalDecision.GetById(model.oBudgetBookCommitment.ApprovalID).Decision;
                    var ApprovalDecision = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision) model.oBudgetBookCommitment.ApprovalID);
                    
                    decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(model.oBudgetBookCommitment.ID);
                    var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);
                    //var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                    var subject = entity.Comitmntno + " has been " + ApprovalDecision + " by the Activity Owner.";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(ActivityOwnerToFocalPointDeclined))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                                entity.FocalPoint.FullName,
                                LoginUser.FullName,
                                entity.title,
                                entity.Comitmntno,
                                model.oBudgetBook.CapexOpex,
                                finalValue,
                                model.oBudgetBook.ActivityCode,
                                model.oBudgetBook.CostObject,
                                model.oBudgetBook.ActivityType,
                                entity.FocalPoint.FullName,
                                repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.LineManager.FullName,
                                repoUsers.GetById(model.oBudgetBook.ActivityOwnerID).Result.FullName,
                                repoUsers.GetById(model.oBudgetBook.SponsorID).Result.FullName,
                                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                                GetBaseUrl(),
                                model.oBudgetBookCommitment.ApprovalComment,
                                ApprovalDecision
                                );
                    await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                    TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully reviewed and Focal point notified.";
                }
            }
            return RedirectToAction("ActivityOwner");
        }

        //LineManagerApproval action below is called by the submit button event on _ApprovalDecision partial view, which is embedded on _CCDetails partial view on LineManager.cshtml view 
        //TODO: Workflow Step 3 : Line Manager Approves/Rejects.
        public async Task<IActionResult> LineManagerApprovalBelowThreshold(CommitmentControlViewModel model)
        {
            //Email Template Stuffs
            //Source: https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var webRoot = _env.WebRootPath; //get wwwroot Folder
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            //IEnumerable<ApprovalDecision> ApprovalStatuses = repoApprovalDecision.GetAll().Result;

            if (LoginUser != null)
            {
                BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);
                int iCurrentYear = DateTime.Today.Year;
                model.oBudgetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();

                if (((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.LineManagerReview)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.Draft)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.TBD)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.CCPanelSession))
                {
                    TempData["Message"] = "Action not successful. Select, from the dropdown, the corresponding status for your action. E.g. Approved as Presented, Provisional Approval...";
                    return RedirectToAction("LineManager");
                }
                else
                {
                    entity.ApprovalID = model.oBudgetBookCommitment.ApprovalID; 
                    entity.ApproverID = LoginUser.ID; //Logged in Line Manager
                    entity.ApprovalComment = model.oBudgetBookCommitment.ApprovalComment;
                    entity.Savings = model.oBudgetBookCommitment.Savings;
                    entity.ModifiedDate = DateTime.Today.Date;

                    bool bRet = await repoBudgetBookCommitments.Update(entity);
                    if (bRet)
                    {
                        decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(model.oBudgetBookCommitment.ID);
                        var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                        //TODO: Deduct the approved value from the budget book, if approved by the Accountable Manager.
                        //TODO: While going fully life, remember to uncomment the codes below.
                        //var ApprovalState = ApprovalStatuses.First(c => entity.ApprovalID == c.ID);
                        //if (ApprovalState.Decision.Contains("APPRO")) //TODO: In the future, you may replace this APPRO with something else. Everywhere you have used it.
                        if (entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented || entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications || entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) //TODO: In the future, you may replace this APPRO with something else. Everywhere you have used it.
                        {
                            BudgetBook entity2 = await repoBudgetBook.GetById(entity.BudgetBookID);
                            entity2.NAPIMSBUDGETFDollar = entity2.NAPIMSBUDGETFDollar - (decimal)CurentTotalExpendedFromBudgetLine;

                            await repoBudgetBook.Update(entity2);
                        }

                        List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                        List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                        AppUsers AccountableManager = await repoUsers.GetById(entity.SponsorID);
                        AppUsers ActivityOwner = await repoUsers.GetById(entity.ActivityOwnerID);
                        AppUsers LineManager = await repoUsers.GetById(entity.LineManagerID);
                        AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);

                        IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                        //TODO: Line Manager to Send mail to Focal Point, copy Activity owner and Accountable Manager, using MailKit
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));
                        mailTo.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(AccountableManager.FullName, AccountableManager.UserMail, AccountableManager.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));

                        var AccountableManagerToFocalPoint = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "AccountableManagerToFocalPoint.html";

                        structUserMailIdx mailFrom = new structUserMailIdx
                        {
                            m_sUserMail = LoginUser.UserMail,
                            m_sUserName = LoginUser.FullName
                        };

                        //var ApprovalDecision = repoApprovalDecision.GetById(model.oBudgetBookCommitment.ApprovalID).Decision;
                        var ApprovalDecision = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)model.oBudgetBookCommitment.ApprovalID);

                        var subject = entity.Comitmntno + " has been " + ApprovalDecision + " by Line Manager.";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(AccountableManagerToFocalPoint))
                        {
                            builder.HtmlBody = SourceReader.ReadToEnd();
                        }

                        string messageBody = string.Format(builder.HtmlBody,
                                    entity.FocalPoint.FullName,
                                    LoginUser.FullName,
                                    entity.title,
                                    entity.Comitmntno,
                                    model.oBudgetBook.CapexOpex,
                                    finalValue,
                                    model.oBudgetBook.ActivityCode,
                                    model.oBudgetBook.CostObject,
                                    model.oBudgetBook.ActivityType,
                                    entity.FocalPoint.FullName,
                                    repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.LineManager.FullName,
                                    repoUsers.GetById(model.oBudgetBook.ActivityOwnerID).Result.FullName,
                                    repoUsers.GetById(model.oBudgetBook.SponsorID).Result.FullName,
                                    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                                    GetBaseUrl(),
                                    ApprovalDecision
                                    );
                        await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                        TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully reviewed and Focal point notified.";
                    }
                }
            }
            return RedirectToAction("LineManager");
        }

        public async Task<IActionResult> LineManagerApprovalAboveThreshold(CommitmentControlViewModel model)
        {
            //Email Template Stuffs
            //Source: https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var webRoot = _env.WebRootPath; //get wwwroot Folder
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            if (LoginUser != null)
            {
                BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);
                int iCurrentYear = DateTime.Today.Year;
                model.oBudgetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();

                if (((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.ApprovedAsPresented)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.ApprovedWithModifications)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.ProvisionalApproval)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.LineManagerReview)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.Draft)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.TBD)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.CCPanelSession))
                {
                    TempData["Message"] = "Action not successful. Select, from the dropdown, the corresponding status for your action. E.g. Approved as Presented, Provisional Approval...";
                    return RedirectToAction("LineManager");
                }
                else
                {
                    entity.ApprovalID = model.oBudgetBookCommitment.ApprovalID; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("DECLI")).FirstOrDefault().ID; 
                    entity.ApprovalComment = model.oBudgetBookCommitment.ApprovalComment;
                    entity.Savings = model.oBudgetBookCommitment.Savings;
                    entity.ModifiedDate = DateTime.Today.Date;

                    bool bRet = await repoBudgetBookCommitments.Update(entity);
                    if (bRet)
                    {
                        decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(model.oBudgetBookCommitment.ID);
                        var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                        List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                        List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                        AppUsers AccountableManager = await repoUsers.GetById(entity.SponsorID);
                        AppUsers ActivityOwner = await repoUsers.GetById(entity.ActivityOwnerID);
                        AppUsers LineManager = await repoUsers.GetById(entity.LineManagerID);
                        AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);

                        IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                        //TODO: Line Manager to Send mail to Focal Point, copy Activity owner and Accountable Manager, using MailKit
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));
                        mailTo.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(AccountableManager.FullName, AccountableManager.UserMail, AccountableManager.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));

                        var AccountableManagerToFocalPointDeclined = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "AccountableManagerToFocalPointDeclined.html";

                        structUserMailIdx mailFrom = new structUserMailIdx
                        {
                            m_sUserMail = LoginUser.UserMail,
                            m_sUserName = LoginUser.FullName
                        };


                        //var ApprovalDecision = repoApprovalDecision.GetById(model.oBudgetBookCommitment.ApprovalID).Decision;
                        var ApprovalDecision = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)model.oBudgetBookCommitment.ApprovalID);

                        var subject = entity.Comitmntno + " has been " + ApprovalDecision + " by the Line Manager.";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(AccountableManagerToFocalPointDeclined))
                        {
                            builder.HtmlBody = SourceReader.ReadToEnd();
                        }

                        string messageBody = string.Format(builder.HtmlBody,
                                    entity.FocalPoint.FullName,
                                    LoginUser.FullName,
                                    entity.title,
                                    entity.Comitmntno,
                                    model.oBudgetBook.CapexOpex,
                                    finalValue,
                                    model.oBudgetBook.ActivityCode,
                                    model.oBudgetBook.CostObject,
                                    model.oBudgetBook.ActivityType,
                                    entity.FocalPoint.FullName,
                                    repoActivityCode.GetById(model.oBudgetBook.ActivityCodeID).Result.LineManager.FullName,
                                    repoUsers.GetById(model.oBudgetBook.ActivityOwnerID).Result.FullName,
                                    repoUsers.GetById(model.oBudgetBook.SponsorID).Result.FullName,
                                    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                                    GetBaseUrl(),
                                    model.oBudgetBookCommitment.ApprovalComment,
                                    ApprovalDecision
                                    );
                        await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                        TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully reviewed and Focal point notified.";
                    }
                }
            }
            return RedirectToAction("LineManager");
        }

        //TODO: Work Flow Step 4 : Line Manager sends for CC Panel's approval/review. This is called from JavaScript on _CostBreakdownList.cshtml partial view
        //TODO: When sent for CCPanel's approval by Line Manager, the UserId of the Accountable Manager is entered into the table 
        //so that the Line Manager will not be seeing the item in the Pending in-tray.
        //So, in the LoadLMPendingCommitments action, check if the ApproverID is null and when not null.
        //When null, the item should show in the in-tray, when not null it means it has been sent for approval.
        public async Task<IActionResult> SendForCCPApproval(long? id)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            if (LoginUser != null)
            {
                BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(id);
                var BudGetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();
                entity.ApprovalID = (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession; //repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("CCPANEL")).FirstOrDefault().ID;
                entity.ModifiedDate = DateTime.Now.Date;

                List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    AppUsers AccountableManager = await repoUsers.GetById(BudGetBook.SponsorID);
                    AppUsers ActivityOwner = await repoUsers.GetById(BudGetBook.ActivityOwnerID);
                    AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);
                    
                    //Note: the Admins are the CCP Panel group
                    IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));

                    decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(id);
                    var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                    foreach (var admin in Admins)
                    {
                        mailTo.Add(new structUserMailIdx(admin.FullName, admin.UserMail, admin.ID.ToString()));
                    }

                    mailCopy.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));
                    mailCopy.Add(new structUserMailIdx(AccountableManager.FullName, AccountableManager.UserMail, AccountableManager.ID.ToString()));

                    //TODO: Send mail to Line Manager, copy Activity Owner using MailKit
                    var LineManagerToAccountableManager = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "LineManegerToAccountableManager.html";

                    structUserMailIdx mailFrom = new structUserMailIdx
                    {
                        m_sUserMail = LoginUser.UserMail,
                        m_sUserName = LoginUser.FullName
                    };

                    var subject = entity.Comitmntno + " is awaiting your review/approval.";
                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(LineManagerToAccountableManager))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string messageBody = string.Format(builder.HtmlBody,
                        "CC Panel members",
                        FocalPoint.FullName,
                        entity.title,
                        entity.Comitmntno,
                        BudGetBook.CapexOpex,
                        finalValue,
                        BudGetBook.ActivityCode,
                        BudGetBook.CostObject,
                        BudGetBook.ActivityType,
                        FocalPoint.FullName,
                        LoginUser.FullName,
                        BudGetBook.ActivityOwner,
                        BudGetBook.Sponsor,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        GetBaseUrl()
                        );

                    await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                    TempData["Message"] = "Commitment " + entity.Comitmntno + " Successfully sent for CC Panel's approval.";
                }
            }
            else
            {
                TempData["Message"] = "Your session has expired. Kindly relogin into Commitment control";
            }
            return RedirectToAction("LineManager");
        }

        //CCPanelApproval action below is called by the submit button event on _ApprovalDecision partial view, which is embedded on _CCDetails partial view on CCPSession.cshtml view 
        //TODO: Workflow Step 5 : CCPanel (Admins) Approve/Reject.
        public async Task<IActionResult> CCPanelApproval(CommitmentControlViewModel model, int? iYear)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
            //IEnumerable<ApprovalDecision> ApprovalStatuses = repoApprovalDecision.GetAll().Result;

            if (LoginUser != null)
            {
                BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);
                model.oBudgetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();

                if (((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.LineManagerReview)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.Draft)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.TBD)
                    || ((int)model.oBudgetBookCommitment.ApprovalID == (int)StatusReporter.Status.CCPanelSession))
                {
                    TempData["Message"] = "Action not successful. Select, from the dropdown, the corresponding status for your action. E.g. Approved as Presented, Provisional Approval...";
                    return RedirectToAction("CCPSession");
                }
                else
                {
                    entity.ApproverID = entity.SponsorID;
                    entity.ApprovalID = model.oBudgetBookCommitment.ApprovalID;
                    entity.ApprovalComment = model.oBudgetBookCommitment.ApprovalComment;
                    entity.Savings = model.oBudgetBookCommitment.Savings;
                    entity.ModifiedDate = DateTime.Today.Date;

                    List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
                    List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

                    bool bRet = await repoBudgetBookCommitments.Update(entity);
                    if (bRet)
                    {
                        decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(model.oBudgetBookCommitment.ID);
                        var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                        //TODO: Deduct the approved value from the budget book, if approved by the Accountable Manager.
                        //TODO: While going fully life, remember to uncomment the codes below.
                        //var ApprovalState = ApprovalStatuses.First(c => entity.ApprovalID == c.ID);
                        if (entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented || entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications || entity.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) //TODO: In the future, you may replace this APPRO with something else. Everywhere you have used it.
                        {
                            BudgetBook entity2 = await repoBudgetBook.GetById(entity.BudgetBookID);
                            entity2.NAPIMSBUDGETFDollar = entity2.NAPIMSBUDGETFDollar - (decimal)CurentTotalExpendedFromBudgetLine;

                            await repoBudgetBook.Update(entity2);
                        }

                        //Sender: This is the logged on CCPanel member that supported on behalf of others
                        structUserMailIdx mailFrom = new structUserMailIdx
                        {
                            m_sUserMail = LoginUser.UserMail,
                            m_sUserName = LoginUser.FullName
                        };

                        AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);
                        AppUsers ActivityOwner = await repoUsers.GetById(model.oBudgetBook.ActivityOwnerID);
                        AppUsers LineManager = await repoUsers.GetById(model.oBudgetBook.LineManagerID);
                        AppUsers AccountableManager = await repoUsers.GetById(model.oBudgetBook.SponsorID);

                        mailTo.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(LineManager.FullName, LineManager.UserMail, LineManager.ID.ToString()));
                        mailCopy.Add(new structUserMailIdx(AccountableManager.FullName, AccountableManager.UserMail, AccountableManager.ID.ToString()));

                        //Note: the Admins are the CCP Panel group, should also be copied in the mail
                        IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);
                        foreach (var admin in Admins)
                        {
                            mailCopy.Add(new structUserMailIdx(admin.FullName, admin.UserMail, admin.ID.ToString()));
                        }

                        //TODO: Send mail to Focal Point and copy Activity Owner and Accountable Manager
                        var CCPanelApproval = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "CCPanelApproval.html";

                        //var ApprovalDecision = repoApprovalDecision.GetById(model.oBudgetBookCommitment.ApprovalID).Decision;
                        var ApprovalDecision = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)model.oBudgetBookCommitment.ApprovalID);
                        var subject = entity.Comitmntno + ", Commitment Control has been " + ApprovalDecision + ".";

                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(CCPanelApproval))
                        {
                            builder.HtmlBody = SourceReader.ReadToEnd();
                        }

                        string messageBody = string.Format(builder.HtmlBody,
                                FocalPoint.FullName,
                                ApprovalDecision,
                                entity.title,
                                entity.Comitmntno,
                                model.oBudgetBook.CapexOpex,
                                finalValue,
                                model.oBudgetBook.ActivityCode,
                                model.oBudgetBook.CostObject,
                                model.oBudgetBook.ActivityType,
                                FocalPoint.FullName,
                                model.oBudgetBookCommitment.LineManagerFullName,
                                model.oBudgetBook.ActivityOwner,
                                model.oBudgetBook.Sponsor,
                                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                                GetBaseUrl(),
                                model.oBudgetBookCommitment.ApprovalComment
                                );
                        await  _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                        TempData["Message"] = "CCP approval/review was successful for " + entity.Comitmntno + " commitment, " + ApprovalDecision;
                    }
                }
            }
            return RedirectToAction("CCPSession");
        }

        public async Task<IActionResult> RerouteRequest(CommitmentControlViewModel model)
        {
            BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(model.oBudgetBookCommitment.ID);
            if (model.oBudgetBookCommitment.ActivityOwnerID != -1)
            {
                var iUserIdOwner = entity.ActivityOwnerID;
                var iUserIdAlternate = model.oBudgetBookCommitment.ActivityOwnerID;

                if (entity.ActivityOwnerID != model.oBudgetBookCommitment.ActivityOwnerID) entity.ActivityOwnerID = model.oBudgetBookCommitment.ActivityOwnerID;
                entity.ModifiedDate = DateTime.Today.Date;
                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    RoutingMail(iUserIdOwner, iUserIdAlternate);
                }
            }

            if (model.oBudgetBookCommitment.LineManagerID != -1)
            {
                var iUserIdOwner = entity.LineManagerID;
                var iUserIdAlternate = model.oBudgetBookCommitment.LineManagerID;

                if (entity.LineManagerID != model.oBudgetBookCommitment.LineManagerID) entity.LineManagerID = model.oBudgetBookCommitment.LineManagerID;
                entity.ModifiedDate = DateTime.Today.Date;
                bool bRet = await repoBudgetBookCommitments.Update(entity);
                if (bRet)
                {
                    RoutingMail(iUserIdOwner, iUserIdAlternate);
                }
            }
           
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRerouteRequest(long? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            CommitmentControlViewModel model = new CommitmentControlViewModel();

            model.oBudgetBookCommitment = GetBudgetBookCommitmentById(Id).Result.FirstOrDefault();
            if (model.oBudgetBookCommitment == null)
            {
                return NotFound();
            }

            ViewBag.ActivityOwner = new SelectList((repoUsers.GetAll().Result != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(t => t.FullName) : null, "ID", "FullName", model.oBudgetBookCommitment.ActivityOwnerID);
            ViewBag.LineManager = new SelectList((repoUsers.GetAll().Result != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).OrderBy(t => t.FullName) : null, "ID", "FullName", model.oBudgetBookCommitment.LineManagerID);
            ViewBag.AccountableManager = new SelectList((repoUsers.GetAll().Result != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).OrderBy(t => t.FullName) : null, "ID", "FullName", model.oBudgetBookCommitment.SponsorID);

            return PartialView("_Rerouting", model);
        }

        //TODO: This is used to present commitment control for Approval or rejection, Called from LineManager.cshtml view with actionItem() Javascript function
        public async Task<IActionResult> PresentAction(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CommitmentControlViewModel model = new CommitmentControlViewModel
            //{
            //    oBudgetBookCommitment = GetBudgetBookCommitmentById(id).FirstOrDefault() //.Where(o => o.ID == id).FirstOrDefault()
            //};

            CommitmentControlViewModel model = new CommitmentControlViewModel();
            model.oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault();

            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID); //.Where(o => o.BudgetBookID == model.oBudgetBookCommitment.BudgetBookID);

            model.LstUploadFiles = await GetUploadedFiles(id);
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["approverID"] = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager), "ID", "FullName", model.oBudgetBookCommitment.ApproverID);
            //ViewData["approvalID"] = new SelectList(repoApprovalDecision.GetAll().Result, "ID", "Decision");
            ViewData["approvalID"] = new SelectList(RolesManager.GetApprovalDecisions().OrderBy(o => o.Text), "Value", "Text");
            ViewData["commitmentsID"] = new SelectList(repoBudgetBookCommitments.GetAll().Result, "ID", "title");


            return PartialView("_CCDetails", model);
            //return PartialView("_CCPanelSession", model);
            //return PartialView("_CommitmentControlPresentationForm", model);
        }

        public async Task<IActionResult> PresentAction2(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //CommitmentControlViewModel model = new CommitmentControlViewModel
            //{
            //    oBudgetBookCommitment = GetBudgetBookCommitmentById(id).FirstOrDefault() //.Where(o => o.ID == id).FirstOrDefault()
            //};

            CommitmentControlViewModel model = new CommitmentControlViewModel();
            model.oBudgetBookCommitment = GetBudgetBookCommitmentById(id).Result.FirstOrDefault();
            model.oBudgetBook = GetBudgetBook(model.oBudgetBookCommitment.BudgetBookID).Result.FirstOrDefault();
            model.lstTotalCostBreakdown = GetCostBreakdownByBudgetBookId(model.oBudgetBookCommitment.BudgetBookID); //.Where(o => o.BudgetBookID == model.oBudgetBookCommitment.BudgetBookID);

            model.LstUploadFiles = await GetUploadedFiles(id);
            model.lstCostBreakdown = GetCostBreakdownByCommitmentId(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["approverID"] = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager), "ID", "FullName", model.oBudgetBookCommitment.ApproverID);
            //ViewData["approvalID"] = new SelectList(repoApprovalDecision.GetAll().Result, "ID", "Decision");
            ViewData["approvalID"] = new SelectList(RolesManager.GetApprovalDecisions().OrderBy(o => o.Text), "Value", "Text");
            ViewData["commitmentsID"] = new SelectList(repoBudgetBookCommitments.GetAll().Result, "ID", "title");


            return PartialView("_CCDetails2", model);
            //return PartialView("_CCPanelSession", model);
            //return PartialView("_CommitmentControlPresentationForm", model);
        }

        //TODO: Workflow Step 4 : Accountable Manager (Sponsor) Approves/Rejects. Note: this step take place at UpdateBBCommitmentReview action.
        //This is due to the fact that, the model needs to be accessed, which is not possible with jQuery JAvaScript call. Though, at the level of understanding 
        //while developing this application. More research work is ongoing. Thanks.
        public async Task<IActionResult> Approval(long? id, int? iYear)
        {
            BudgetBookCommitments entity = await repoBudgetBookCommitments.GetById(id);
            //entity.ApprovalID = repoApprovalDecision.GetAll().Result.Where(o => o.Decision.Contains("TBD")).FirstOrDefault().ID;
            entity.ModifiedDate = DateTime.Now.Date;

            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            var BudGetBook = GetBudgetBook(entity.BudgetBookID).Result.FirstOrDefault();

            List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
            List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

            bool bRet = await repoBudgetBookCommitments.Update(entity);
            if (bRet)
            {
                string UserMail = Apps.getFullEmail(User.Identity.Name);
                AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();
                AppUsers AccountableManager = await repoUsers.GetById(BudGetBook.SponsorID);
                AppUsers ActivityOwner = await repoUsers.GetById(BudGetBook.ActivityOwnerID);
                AppUsers LineManager = await repoUsers.GetById(BudGetBook.LineManagerID);
                AppUsers FocalPoint = await repoUsers.GetById(entity.FocalPointID);

                IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

                decimal? CurentTotalExpendedFromBudgetLine = GetTotalSumCostBreakDownByCommitmentId(id);
                var finalValue = stringRoutine.formatAsBankMoney("$", CurentTotalExpendedFromBudgetLine);

                //TODO: Accountable Manager to Send mail to Focal Point, copy Activity owner and Line Manager, using MailKit
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(LoginUser.FullName, LoginUser.UserMail));
                mailTo.Add(new structUserMailIdx(FocalPoint.FullName, FocalPoint.UserMail, FocalPoint.ID.ToString()));
                mailCopy.Add(new structUserMailIdx(ActivityOwner.FullName, ActivityOwner.UserMail, ActivityOwner.ID.ToString()));
                mailCopy.Add(new structUserMailIdx(LineManager.FullName, AccountableManager.UserMail, AccountableManager.ID.ToString()));

                var AccountableManagerToFocalPoint = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "AccountableManagerToFocalPoint.html";

                structUserMailIdx mailFrom = new structUserMailIdx
                {
                    m_sUserMail = LoginUser.UserMail,
                    m_sUserName = LoginUser.FullName
                };

                var subject = entity.Comitmntno + " has been reviewed by Accountable Manager.";
                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(AccountableManagerToFocalPoint))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                string messageBody = string.Format(builder.HtmlBody,
                entity.FocalPoint.FullName,
                repoUsers.GetById(BudGetBook.SponsorID).Result.FullName,
                entity.title,
                entity.Comitmntno,
                BudGetBook.CapexOpex,
                finalValue,
                BudGetBook.ActivityCode,
                BudGetBook.CostObject,
                BudGetBook.ActivityType,
                entity.FocalPoint.FullName,
                repoActivityCode.GetById(BudGetBook.ActivityCodeID).Result.LineManager.FullName,
                repoUsers.GetById(BudGetBook.ActivityOwnerID).Result.FullName,
                repoUsers.GetById(BudGetBook.SponsorID).Result.FullName,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                GetBaseUrl()
                );
                await _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

                TempData["Message"] = "Commitment " + entity.title + " Successfully reviewed and Focal point notified.";
            }
            return RedirectToAction("Review");
        }

        #endregion

        #region  ==================== Region to export report to Excel with JavaScript (jQuery) =================

        //TODO: Source: https://www.codeproject.com/tips/1156485/how-to-create-and-download-file-with-ajax-in-asp-n
        //But converted to MVC Core version by self.
        [HttpPost]
        public JsonResult ExportExcel(int? iSelectedType, int? iWeek)
        {
            string fileName = "BongaCommitmentControl" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
            //save the file to server temp folder
            string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TempFolder" + Path.DirectorySeparatorChar.ToString() + fileName;

            if (iWeek == null)
            {
                TempData["Message"] = "Please, select week to view the report";
                return null;
            }
            else if (iSelectedType == null)
            {
                TempData["Message"] = "Please, select report type to view report.";
                return null;
            }
            else
            {
                int? iYear = DateTime.Today.Year;

                Printing oExcelReport = new Printing();
                oExcelReport = GenerateExcelFile(iYear, iWeek);

                if (iSelectedType == ((int)exportType.A3)) PrintToA3(oExcelReport);
                else if (iSelectedType == ((int)exportType.A4)) PrintToA4(oExcelReport);

                using (var exportData = new MemoryStream(oExcelReport.excel.GetAsByteArray()))
                {
                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    exportData.WriteTo(file);
                    file.Close();
                }

                //var errorMessage = "you can return the errors in here!";
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = "" });
        }

        [HttpGet]
        //[DeleteFileAttribute] //Action Filter, it will auto delete the file after download,  I will explain it later
        public FileStreamResult Download(string file)
        {
            string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TempFolder" + Path.DirectorySeparatorChar.ToString() + file;
            var contentType = GetContentType(file);

            byte[] dfile = System.IO.File.ReadAllBytes(fullPath);

            var content = new MemoryStream(dfile);

            return File(content, contentType, file);
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                //filterContext.HttpContext.Response.Flush();

                //convert the current filter context to file and get the file path
                string filePath = ""; //(filterContext.Result as FilePathResult).FileName;

                //delete the file after download
                System.IO.File.Delete(filePath);
            }
        }

        private Printing GenerateExcelFile(int? iYear, int? iWeek)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            ExcelPackage excel = new ExcelPackage();
            var oWs = excel.Workbook.Worksheets.Add("Bonga Commitment Control Report");
            oWs.TabColor = Color.Black;
            oWs.DefaultRowHeight = 12;

            #region  ======================== Report Header =====================================

            int row = 1;
            oWs.Row(row).Height = 35;
            oWs.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            oWs.Row(row).Style.Font.Bold = true;
            oWs.Row(row).Style.Font.Size = 25;
            oWs.Cells[row, 2, row, 5].Value = "Bonga Commitment Control Report";
            oWs.Cells[row, 2, row, 5].Merge = true;
            oWs.Cells[row, 2].Style.WrapText = true;

            row++;
            oWs.Cells[row, 2, row, 5].Value = "";
            oWs.Cells[row, 2, row, 5].Value = DateTime.Today.Date.ToLongDateString();
            oWs.Row(row).Style.Font.Size = 12;
            oWs.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            oWs.Cells[row, 2, row, 5].Merge = true;
            oWs.Cells[row, 2].Style.WrapText = true;

            row++;
            oWs.Cells[row, 2, row, 6].Value = "";

            #endregion

            #region ======================== Report Body =====================================

            row++;
            oWs.Cells[row, 1].Value = "S/No";
            oWs.Cells[row, 2].Value = "BCC No.";
            oWs.Cells[row, 3].Value = "Activity Description";
            oWs.Cells[row, 4].Value = "Commitment(F$)";

            oWs.Cells[row, 5].Value = "PO Number";
            oWs.Cells[row, 6].Value = "PO Value(F$)";
            oWs.Cells[row, 7].Value = "PR Number";
            oWs.Cells[row, 8].Value = "PR Value(F$)";

            oWs.Cells[row, 9].Value = "Capex / Opex";
            oWs.Cells[row, 10].Value = "UAP Code";
            oWs.Cells[row, 11].Value = "Cost Object";
            oWs.Cells[row, 12].Value = "Activity Name";
            oWs.Cells[row, 13].Value = "Activity";

            oWs.Cells[row, 14].Value = "Focal Point";
            oWs.Cells[row, 15].Value = "Activity Owner";
            oWs.Cells[row, 16].Value = "Line Manager";
            oWs.Cells[row, 17].Value = "Accountable Manager";

            oWs.Cells[row, 18].Value = "Contract"; 
            oWs.Cells[row, 19].Value = "Scope";
            oWs.Cells[row, 20].Value = "Approval Status";
            oWs.Cells[row, 21].Value = "Approval Comments";
            oWs.Cells[row, 22].Value = "Approver";
            oWs.Row(row).Style.Font.Bold = true;

            var cell = oWs.Cells[row, 1, row, 22];
            var border = cell.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.LightSeaGreen);

            row++;
            //List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitments(iYear).Where(o => o.ApprovalStatus.Contains("APPRO") && o.AddedDateWeek == iWeek).ToList();
            List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitments(iYear).Where(o => o.AddedDateWeek == iWeek && (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                                                   || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                                                   || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval
                                                                                                    && o.AddedDateWeek == iWeek)).ToList();




            if (rpt != null)
            {
                DateTime firstDayOfWeek = Computations.FirstDateOfWeek(iYear, iWeek, CultureInfo.CurrentCulture);

                int i = 1;
                foreach (BudgetBookCommitmentViewModel o in rpt)
                {
                    oWs.Row(row).Height = 70;
                    oWs.Cells[row, 1].Value = i++;
                    oWs.Cells[row, 2].Value = o.Comitmntno;
                    oWs.Cells[row, 3].Value = o.title; oWs.Cells[row, 4].Style.WrapText = true;
                    oWs.Cells[row, 4].Value = stringRoutine.formatAsBankMoney(o.Commitment);

                    oWs.Cells[row, 5].Value = o.PONumber;
                    oWs.Cells[row, 6].Value = stringRoutine.formatAsBankMoney(o.POValue);
                    oWs.Cells[row, 7].Value = o.PRNumber;
                    oWs.Cells[row, 8].Value = stringRoutine.formatAsBankMoney(o.PRValue);

                    oWs.Cells[row, 9].Value = o.CapexOpex;
                    oWs.Cells[row, 10].Value = o.UapCode;
                    oWs.Cells[row, 11].Value = o.CostObject;
                    oWs.Cells[row, 12].Value = o.ActivityType; oWs.Cells[row, 9].Style.WrapText = true;
                    oWs.Cells[row, 13].Value = o.Activity; oWs.Cells[row, 10].Style.WrapText = true;

                    oWs.Cells[row, 14].Value = o.FocalPoint;
                    oWs.Cells[row, 15].Value = o.ActivityOwner;
                    oWs.Cells[row, 16].Value = o.LineManagerFullName;
                    oWs.Cells[row, 17].Value = o.Sponsor;

                    oWs.Cells[row, 18].Value = o.Contract; oWs.Cells[row, 15].Style.WrapText = true;
                    oWs.Cells[row, 19].Value = o.Scope; oWs.Cells[row, 16].Style.WrapText = true;

                    oWs.Cells[row, 20].Value = o.ApprovalStatus;
                    oWs.Cells[row, 21].Value = o.ApprovalComment; oWs.Cells[row, 20].Style.WrapText = true;
                    oWs.Cells[row, 22].Value = o.Approver;

                    cell = oWs.Cells[row, 1, row, 22];
                    border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    row++;
                    oWs.Cells[row, 3, row, 8].Value = "Cost Breakdown";
                    oWs.Cells[row, 3, row, 8].Merge = true;
                    oWs.Row(row).Style.Font.Bold = true;

                    cell = oWs.Cells[row, 3, row, 8];
                    border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.LightSeaGreen);

                    row++;
                    oWs.Cells[row, 3].Value = "S/No";
                    oWs.Cells[row, 4].Value = "Description";
                    oWs.Cells[row, 5].Value = "Quantity";
                    oWs.Cells[row, 6].Value = "Rate";
                    oWs.Cells[row, 7].Value = "Fixed Exchange Rate";
                    oWs.Cells[row, 8].Value = "Total";
                    oWs.Row(row).Style.Font.Bold = true;

                    cell = oWs.Cells[row, 3, row, 8];
                    border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //fill = cell.Style.Fill;
                    //fill.PatternType = ExcelFillStyle.Solid;
                    //fill.BackgroundColor.SetColor(Color.LightSeaGreen);


                    row++;
                    var details = GetCostBreakdownByCommitmentId(o.ID);
                    if (details != null)
                    {
                        int k = 1;
                        decimal? totalSum = 0;
                        foreach (ActivityDetailsViewModel e in details)
                        {
                            totalSum += e.Calculated;
                            oWs.Cells[row, 3].Value = k++;
                            oWs.Cells[row, 4].Value = e.Description;
                            oWs.Cells[row, 5].Value = e.Quantity;
                            oWs.Cells[row, 6].Value = e.Rate;
                            oWs.Cells[row, 7].Value = e.FixedExchangeRate;
                            oWs.Cells[row, 8].Value = stringRoutine.formatAsBankMoney(e.Calculated);

                            cell = oWs.Cells[row, 3, row, 8];
                            border = cell.Style.Border;
                            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            row++;
                        }
                        oWs.Cells[row, 3, row, 7].Value = ""; oWs.Cells[row, 3, row, 7].Merge = true;
                        oWs.Cells[row, 8].Value = stringRoutine.formatAsBankMoney(totalSum);
                        cell = oWs.Cells[row, 3, row, 8];
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    //oWs.Cells[row, 1, row, 8].Merge = true;
                    //oWs.Cells[row, 1].Style.WrapText = true;
                    //Border
                    //cell = oWs.Cells[row, 1, row, 8];
                    //border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }

            row++;

            oWs.Column(1).Width = 5;
            oWs.Column(2).Width = 18;
            oWs.Column(3).Width = 35;
            oWs.Column(4).Width = 18;
            oWs.Column(5).Width = 18;
            oWs.Column(6).Width = 18;
            oWs.Column(7).Width = 18;
            oWs.Column(8).Width = 18;
            oWs.Column(9).Width = 18;
            oWs.Column(10).Width = 18;
            oWs.Column(11).Width = 18;
            oWs.Column(12).Width = 45;
            oWs.Column(13).Width = 45;
            oWs.Column(14).Width = 18;
            oWs.Column(15).Width = 18;
            oWs.Column(16).Width = 18;
            oWs.Column(17).Width = 18;
            oWs.Column(18).Width = 60;
            oWs.Column(19).Width = 60;
            oWs.Column(20).Width = 60;
            oWs.Column(21).Width = 60;
            oWs.Column(22).Width = 18;
            
            Printing ePrint = new Printing();
            ePrint.excel = excel;
            ePrint.oWs = oWs;

            return ePrint;

            #endregion ======================== Report Body =====================================
        }

        private Printing PrintToA3(Printing oPrint)
        {
            // Set printer settings
            oPrint.oWs.PrinterSettings.PaperSize = ePaperSize.A3;
            oPrint.oWs.PrinterSettings.Orientation = eOrientation.Portrait;
            //oPrint.oWs.PrinterSettings.FitToPage = true;
            oPrint.oWs.PrinterSettings.FitToHeight = 1;
            oPrint.oWs.PrinterSettings.TopMargin = .5M;
            oPrint.oWs.PrinterSettings.FooterMargin = .5M;
            oPrint.oWs.PrinterSettings.LeftMargin = .5M;
            oPrint.oWs.PrinterSettings.RightMargin = .5M;
            oPrint.oWs.Column(30).PageBreak = true;
            oPrint.oWs.PrinterSettings.Scale = 63;

            return oPrint;
        }

        private Printing PrintToA4(Printing oPrint)
        {
            // Set printer settings
            oPrint.oWs.PrinterSettings.PaperSize = ePaperSize.A4;
            oPrint.oWs.PrinterSettings.Orientation = eOrientation.Portrait;
            //oPrint.oWs.PrinterSettings.FitToPage = true;
            oPrint.oWs.PrinterSettings.FitToHeight = 1;
            oPrint.oWs.PrinterSettings.TopMargin = .55M;
            oPrint.oWs.PrinterSettings.FooterMargin = .75M;
            oPrint.oWs.PrinterSettings.LeftMargin = .5M;
            oPrint.oWs.PrinterSettings.RightMargin = .5M;
            oPrint.oWs.Column(30).PageBreak = true;
            oPrint.oWs.PrinterSettings.Scale = 41;
            
            return oPrint;           
        }

        #endregion

        #region ==================== Region to Export Report to PDF ==============================

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsWeeklyReport(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) 
                                                                         && o.iYear == iYear).ToList().OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //TODO: Source: https://code-maze.com/create-pdf-dotnetcore/
        public string GetHTMLString(int? iWeek)
        {
            var sb = new StringBuilder();
            int? iYear = DateTime.Today.Year;
            //List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitments(iYear).Where(o => o.ApprovalStatus.Contains("APPRO") && o.AddedDateWeek == iWeek).ToList();
            List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitmentsWeeklyReport(iYear).Result.Where(o => o.AddedDateWeek == iWeek).ToList();

            if (rpt != null)
            {
                DateTime firstDayOfWeek = Computations.FirstDateOfWeek(iYear, iWeek, CultureInfo.CurrentCulture);
                string imagesPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Images" + Path.DirectorySeparatorChar.ToString() + "BCCLogo.png";
                var picAddress = imagesPath;
                sb.AppendFormat(@"<img src='{0}' alt='shellLogo'/>", picAddress);
                sb.Append("<H3>" + string.Format("Week {0},  {1}", iWeek, firstDayOfWeek.ToString("MMM d, yyyy")) + "</H3>");
                ReportFormatString2(sb, rpt);
            } 
            return sb.ToString();
        }

        public IEnumerable<BudgetBookCommitmentViewModel> GetBudgetBookCommitmentsWeeklyPreReadReport(int? iYear)
        {
            try
            {
                int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
                var FocalPoints = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.FocalPoint
                                    || o.RoleId == (int)enuRole.Admin
                                    || o.RoleId == (int)enuRole.AccountableManager
                                    || o.RoleId == (int)enuRole.ActivityOwner
                                    || o.RoleId == (int)enuRole.Corporate
                                    || o.RoleId == (int)enuRole.LineManager).ToList();

                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD
                                                                         || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.PendingCCPanelSession)
                                                                         && o.iYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    var ApprovalState = new ApprovalDecision();
                    var FocalPoint = new AppUsers();

                    //if (ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID) != null) ApprovalState = ApprovalStatuses.FirstOrDefault(c => entity.ApprovalID == c.ID);
                    if (FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID) != null) FocalPoint = FocalPoints.FirstOrDefault(c => entity.FocalPointID == c.ID);

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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision)entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetHTMLStringPreread(int? iWeek)
        {
            var sb = new StringBuilder();
            int? iYear = DateTime.Today.Year;
            //List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitments(iYear).Where(o => o.AddedDateWeek == iWeek && (o.ApprovalStatus.Contains("APPRO") || o.ApprovalStatus.Contains("MANAGER") || o.ApprovalStatus.Contains("TBD") || o.ApprovalStatus.Contains("CCPANEL"))).ToList();
            List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitmentsWeeklyPreReadReport(iYear).Where(o => o.AddedDateWeek == iWeek).ToList();

            if (rpt != null)
            {
                DateTime firstDayOfWeek = Computations.FirstDateOfWeek(iYear, iWeek, CultureInfo.CurrentCulture);
                string imagesPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Images" + Path.DirectorySeparatorChar.ToString() + "BCCLogo.png";
                var picAddress = imagesPath;
                sb.AppendFormat(@"<img src='{0}' alt='shellLogo'/>", picAddress);
                sb.Append("<H3>" + string.Format("Week {0},  {1}", iWeek, firstDayOfWeek.ToString("MMM d, yyyy")) + "</H3>");
                ReportFormatString2(sb, rpt);
            }
            return sb.ToString();
        }

        public async Task<IEnumerable<BudgetBookCommitmentViewModel>> GetBudgetBookCommitmentsVariance(DateTime? Date1, DateTime? Date2)
        {
            try
            {
                var BudgetBooke = repoBudgetBook.GetAll().Result.ToList();
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

                var result = repoBudgetBookCommitments.GetAll().Result.Where(o => (o.AddedDate >= Date1 && o.AddedDate <= Date2)
                                                                     && (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                      || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                      || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)).OrderBy(o => o.ID).ToList().Select(entity =>
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

                    if (BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID) != null) BudgetBook = BudgetBooke.FirstOrDefault(c => entity.BudgetBookID == c.ID);
                    if (BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID) != null) BudgetBasis = BudgetBases.FirstOrDefault(c => BudgetBook.BudgetBasisID == c.ID);
                    if (Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID) != null) Activity = Activities.FirstOrDefault(c => BudgetBook.ActivityID == c.ID);
                    if (ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID) != null) ActivityName = ActivityNames.FirstOrDefault(c => BudgetBook.ActivityTypeID == c.ID);
                    if (Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID) != null) Scope = Scopes.FirstOrDefault(c => BudgetBook.ScopeID == c.ID);
                    if (Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID) != null) Contract = Contracts.FirstOrDefault(c => BudgetBook.ContractID == c.ID);
                    if (UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID) != null) UapCode = UapCodes.FirstOrDefault(c => BudgetBook.UapCodeID == c.ID);
                    if (UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID) != null) Uaprollupcode = UapRollUpCodes.FirstOrDefault(c => BudgetBook.UapRollUpCodeID == c.ID);
                    if (ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID) != null) ActivityCode = ActivityCodes.FirstOrDefault(c => BudgetBook.ActivityCodeID == c.ID);
                    if (WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID) != null) wbs = WBSes.FirstOrDefault(c => BudgetBook.wbsID == c.ID);
                    if (CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID) != null) CapexOpex = CapexOpexs.FirstOrDefault(c => BudgetBook.CapexOpexID == c.ID);

                    decimal? TotalCommitment = GetTotalSumCostBreakDownByCommitmentId(entity.ID);

                    return new BudgetBookCommitmentViewModel
                    {
                        CapexOpexID = BudgetBook.CapexOpexID,
                        CapexOpex = CapexOpex.CapexOpex,
                        DirectAllocated = BudgetBook.DirectAllocated,
                        sDirectAllocated = ((enuDirectAllocated)(Convert.ToInt16(BudgetBook.DirectAllocated))).ToString(),

                        PONumber = entity.PONumber,
                        POValue = entity.POValue,
                        PRNumber = entity.PRNumber,
                        PRValue = entity.PRValue,

                        UapCodeID = BudgetBook.UapCodeID,
                        UapCode = UapCode.UapCodeDesc,
                        UapRollUpCodeID = BudgetBook.UapRollUpCodeID,
                        UapRollUpCode = Uaprollupcode.UapRollUpCodeDesc,
                        ActivityTypeID = BudgetBook.ActivityTypeID,
                        ActivityType = ActivityName.ActivityName,
                        ActivityCodeID = BudgetBook.ActivityCodeID,
                        ActivityCode = ActivityCode.ActivityCodeDesc,
                        wbsID = BudgetBook.wbsID,
                        CostObject = wbs.CostObjects,
                        ActivityID = BudgetBook.ActivityID,
                        Activity = Activity.Description,

                        ScopeID = BudgetBook.ScopeID,
                        Scope = Scope.Purpose,
                        ContractID = BudgetBook.ContractID,
                        Contract = Contract.ContractName,
                        BudgetBasisID = BudgetBook.BudgetBasisID,
                        BudgetBasis = BudgetBasis.BudgetBase,
                        //OPYearBudgetFDollar = BudgetBook.OPYearBudgetFDollar,
                        NAPIMSBUDGETFDollar = BudgetBook.NAPIMSBUDGETFDollar,
                        //Q1FYLEFDollar = BudgetBook.Q1FYLEFDollar,
                        BudgetBookID = BudgetBook.ID,

                        ID = entity.ID,
                        title = entity.title,
                        CCPSessionDate = entity.CCPSessionDate,
                        Comitmntno = entity.Comitmntno,

                        Commitment = TotalCommitment,
                        FocalPointID = entity.FocalPointID,
                        FocalPoint = repoUsers.GetById(entity.FocalPointID).Result.FullName,

                        ApprovalID = entity.ApprovalID,
                        sApprovalID = StatusReporter.StatusDescription((StatusReporter.Status)entity.ApprovalID),
                        ApprovalStatus = ApprovalDecisionType.ApprovalDecisionDesc((ApprovalDecisionType.enuApprovalDecision) entity.ApprovalID),
                        ApprovalComment = entity.ApprovalComment,
                        //ApproverID = entity.ApproverID,
                        Savings = entity.Savings,
                        iYear = entity.iYear,


                        ActivityOwnerID = entity.ActivityOwnerID,
                        ActivityOwner = (entity.ActivityOwnerID == null) ? "" : repoUsers.GetById(entity.ActivityOwnerID).Result.FullName,

                        LineManagerID = entity.LineManagerID,
                        LineManagerFullName = (entity.LineManagerID == null) ? "" : repoUsers.GetById(entity.LineManagerID).Result.FullName,

                        SponsorID = entity.SponsorID,
                        Sponsor = (entity.SponsorID == null) ? "" : repoUsers.GetById(entity.SponsorID).Result.FullName,

                        ApproverID = entity.ApproverID,
                        Approver = (entity.ApproverID == null) ? "" : repoUsers.GetById(entity.ApproverID).Result.FullName,

                        AddedDate = entity.AddedDate,
                        AddedDateWeek = stringRoutine.getWeek(entity.AddedDate),

                        justification = entity.justification,
                        threat = entity.threat,
                        contractnovendor = entity.contractnovendor,
                        sPeriodfrom = entity.sPeriodfrom,

                        groupID = entity.groupID,
                        Group = (entity.groupID == null) ? "" : repoPurchaseGroup.GetById(entity.groupID).Result.GroupName,
                        teamID = entity.teamID,
                        sTeam = (entity.teamID == null) ? "" : repoTeam.GetById(entity.teamID).Result.TeamName,
                        typeID = entity.typeID,
                        PlannedEmmergency = (entity.typeID == null) ? "" : repoPlannedEmmergency.GetById(entity.typeID).Result.PlanEmmerType,
                        statusID = entity.statusID,
                        Status = (entity.statusID == null) ? "" : repoStatus.GetById(entity.statusID).Result.ReqstStatus,
                        vehicleID = entity.vehicleID,
                        VehicleProcurement = (entity.vehicleID == null) ? "" : repoVehicle.GetById(entity.vehicleID).Result.VehicleName,
                        WorkFlowType = (ActivityCode.ActivityCodeWorkStreamID != null) ? repoActivityCodeWorkStream.GetById(ActivityCode.ActivityCodeWorkStreamID).Result.WorkFlowType : (int)WorkFlowTypes.enuWorkFlowType.FP_AO_LM_CCP,
                    };
                }).ToList().OrderByDescending(o => o.ID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetHTMLVarianceReportString(DateTime Date1, DateTime Date2)
        {
            var sb = new StringBuilder();
            List<BudgetBookCommitmentViewModel> rpt = GetBudgetBookCommitmentsVariance(Date1, Date2).Result.ToList();

            if (rpt != null)
            {
                //DateTime firstDayOfWeek = Computations.FirstDateOfWeek(iYear, iWeek, CultureInfo.CurrentCulture);
                string imagesPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Images" + Path.DirectorySeparatorChar.ToString() + "BCCLogo.png";
                var picAddress = imagesPath;
                sb.AppendFormat(@"<img src='{0}' alt='shellLogo'/>", picAddress);
                sb.Append("<H3>" + string.Format("Between {0}, and {1}", Date1.ToString("MMM d, yyyy"), Date2.ToString("MMM d, yyyy")) + "</H3>");
                ReportFormatString2(sb, rpt);
            }
            return sb.ToString();
        }

        public StringBuilder ReportFormatString(StringBuilder sb, List<BudgetBookCommitmentViewModel> rpt)
        {
            var PRValue = 0.0M;
            var POValue = 0.0M;
            decimal? Commitments = 0.0M;
            //decimal? Savings = 0.0M;

            //S.No/
            int i = 1;
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <table align='left'>
                                    <tr>
                                        <th>S.No</th>
                                        <th>Capex/Opex</th>
                                        <th>UAP Code</th>
                                        <th>Cost Object</th>
                                        <th>Activity Name</th>
                                        <th>Activity</th>
                                        <th>Activity Description</th>
                                        <th>Focal Point</th>
                                        <th>Line Manager</th>
                                        <th>Business Justification</th>
                                        <th>Contract</th>
                                        <th>PR Number</th>
                                        <th>PR Value (F$)</th>
                                        <th>PO Value (F$)</th>
                                        <th>Commitment (F$)</th>
                                        <th>PR/PO variance %</th>
                                        <th>Approval Decision</th>
                                        <th>Approval Decision comment</th>
                                        <th>Approver</th>
                                        <th>...</th>
                                    </tr>");

            foreach (var o in rpt)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    <td>{7}</td>
                                    <td>{8}</td>
                                    <td>{9}</td>
                                    <td>{10}</td>
                                    <td>{11}</td>
                                    <td align='right'>{12}</td>
                                    <td align='right'>{13}</td>
                                    <td align='right'>{14}</td>
                                    <td>{15}</td>
                                    <td>{16}</td>
                                    <td>{17}</td>
                                    <td>{18}</td>
                                    <td>{19}</td>
                                  </tr>", i++,
                              o.CapexOpex,
                              o.UapCode,
                              o.CostObject,
                              o.ActivityType,
                              o.Activity,
                              o.title,
                              o.FocalPoint,
                              o.LineManagerFullName,
                              o.justification,
                              o.contractnovendor,
                              o.PRNumber,
                              stringRoutine.formatAsBankMoney(o.PRValue),
                              stringRoutine.formatAsBankMoney(o.POValue),
                              stringRoutine.formatAsBankMoney(o.Commitment),
                              "",
                              o.ApprovalStatus,
                              o.ApprovalComment,
                              o.Approver, 
                              o.Comitmntno);

                PRValue += o.PRValue;
                POValue += o.POValue;
                Commitments += o.Commitment;
            }

            sb.AppendFormat(@"<tr>
                                <th colspan='12' align='right'>Total</th>
                                <th align='right'>{12}</th>
                                <th align='right'>{13}</th>
                                <th align='right'>{14}</th>
                                <th></th>
                                <th></th>
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>", "", "", "", "", "", "", "", "", "", "", "", "", stringRoutine.formatAsBankMoney(PRValue), stringRoutine.formatAsBankMoney(POValue), stringRoutine.formatAsBankMoney(Commitments), "", "", "", "");
            sb.Append(@"</table>
                        </body>
                    </html>");
            return sb;
        }

        public StringBuilder ReportFormatString2(StringBuilder sb, List<BudgetBookCommitmentViewModel> rpt)
        {
            var PRValue = 0.0M;
            var POValue = 0.0M;
            decimal? Commitments = 0.0M;
            //decimal? Savings = 0.0M;

            //S.No/
            int i = 1;
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <table align='left'>
                                    <tr>
                                        <th>S.No</th>
                                        <th>Capex/Opex</th>
                                        <th>UAP Code</th>
                                        <th>Cost Object</th>
                                        <th>Activity Name</th>
                                        <th>Activity</th>
                                        <th>Activity Description</th>
                                        <th>Focal Point</th>
                                        <th>Line Manager</th>
                                        <th>Business Justification</th>
                                        <th>Contract</th>
                                        <th>PR Number</th>
                                        <th>PR Value (F$)</th>
                                        <th>PO Value (F$)</th>
                                        <th>Commitment (F$)</th>
                                        <th>PR/PO variance %</th>
                                        <th>Approval Decision</th>
                                        <th>Approval Decision comment</th>
                                        <th>Approver</th>
                                        <th>...</th>
                                    </tr>");

            foreach (var o in rpt)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    <td>{7}</td>
                                    <td>{8}</td>
                                    <td>{9}</td>
                                    <td>{10}</td>
                                    <td>{11}</td>
                                    <td align='right'>{12}</td>
                                    <td align='right'>{13}</td>
                                    <td align='right'>{14}</td>
                                    <td>{15}</td>
                                    <td>{16}</td>
                                    <td>{17}</td>
                                    <td>{18}</td>
                                    <td>{19}</td>
                                  </tr>", i++,
                              o.CapexOpex,
                              o.UapCode,
                              o.CostObject,
                              o.ActivityType,
                              o.Activity,
                              o.title,
                              o.FocalPoint,
                              o.LineManagerFullName,
                              o.justification,
                              o.contractnovendor,
                              o.PRNumber,
                              stringRoutine.formatAsBankMoney(o.PRValue),
                              stringRoutine.formatAsBankMoney(o.POValue),
                              stringRoutine.formatAsBankMoney(o.Commitment),
                              "",
                              o.ApprovalStatus,
                              o.ApprovalComment, 
                              o.Approver,
                              o.Comitmntno);

                PRValue += o.PRValue;
                POValue += o.POValue;
                Commitments += o.Commitment;
            }

            sb.AppendFormat(@"<tr>
                                <th colspan='12' align='right'>Total</th>
                                <th align='right'>{12}</th>
                                <th align='right'>{13}</th>
                                <th align='right'>{14}</th>
                                <th></th>
                                <th></th>
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>", "", "", "", "", "", "", "", "", "", "", "", "", stringRoutine.formatAsBankMoney(PRValue), stringRoutine.formatAsBankMoney(POValue), stringRoutine.formatAsBankMoney(Commitments), "", "", "", "");
            sb.Append(@"</table>
                        </body>
                    </html>");
            return sb;
        }

        [HttpPost]
        public JsonResult ExportToPDF(int? iWeek)
        { 
            string fileName = "BongaCommitmentControl" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            if (iWeek == null)
            {
                TempData["Message"] = "Please, select week to view the report";
                return null;
            }
            else
            {
                string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TempFolder" + Path.DirectorySeparatorChar.ToString() + fileName;
                string fullPathStyle = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "assets" + Path.DirectorySeparatorChar.ToString() + "styles.css";

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A3,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Bonga Commitment Control PDF Report",
                    Out = fullPath
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = GetHTMLString(iWeek),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = fullPathStyle },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Futura Medium", FontSize = 9, Right = "Page [page] of [toPage]", Line = true }
                    
                    //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = "" });
            //return Ok("Successfully created PDF document.");
        }

        [HttpPost]
        public JsonResult ExportPrereadToPDF(int? iWeek)
        {
            string fileName = "BongaCommitmentControl" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            if (iWeek == null)
            {
                TempData["Message"] = "Please, select week to view the report";
                return null;
            }
            else
            {
                string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TempFolder" + Path.DirectorySeparatorChar.ToString() + fileName;
                string fullPathStyle = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "assets" + Path.DirectorySeparatorChar.ToString() + "styles.css";

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A3,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Bonga Commitment Control PDF Report",
                    Out = fullPath
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = GetHTMLStringPreread(iWeek),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = fullPathStyle },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Futura Medium", FontSize = 9, Right = "Page [page] of [toPage]", Line = true }

                    //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = "" });
            //return Ok("Successfully created PDF document.");
        }


        [HttpPost]
        public JsonResult ExportVarianceReportToPDF(DateTime Date1, DateTime Date2)
        {
            string fileName = "BongaCommitmentControl" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            if (Date1 == null || Date2 == null)
            {
                TempData["Message"] = "Please, select Date from and To, to view report";
                return null;
            }
            else
            {
                string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "TempFolder" + Path.DirectorySeparatorChar.ToString() + fileName;
                string fullPathStyle = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "assets" + Path.DirectorySeparatorChar.ToString() + "styles.css";

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A3,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Bonga Commitment Control PDF Report",
                    Out = fullPath
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = GetHTMLVarianceReportString(Date1, Date2),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = fullPathStyle },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Futura Medium", FontSize = 9, Right = "Page [page] of [toPage]", Line = true }

                    //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = "" });
            //return Ok("Successfully created PDF document.");
        }


        #endregion

        #region  ==================== Reporting  ====================

        //public ActionResult Read_Report_by_week([DataSourceRequest] DataSourceRequest request, long? IdCapexOpex, long? IdCostObject, long? IdStatus, long? IdActivityOwner, long? IdActivityCode, long? IdActivityName, long? IdScope, long? IdBudgetBasis, DateTime? CCPSessionDateFrom, DateTime? CCPSessionDateTo, int? iYear, int? iWeek)
        public async Task<ActionResult> Read_Report_by_week([DataSourceRequest] DataSourceRequest request, int? iYear, int? iWeek)
        {
            //TBD, APPRO, MANAGER
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
            var model = Session.GetObjectFromJson<IEnumerable<BudgetBookCommitmentViewModel>>("Reporters");
            model = GetBudgetBookCommitmentsWeeklyReport(iYear).Result.Where(o => o.AddedDateWeek == iWeek);
            
            //GetBudgetBookCommitments(iYyear).Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
            //                                                 || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
            //                                                 || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval
            //                                                 && o.AddedDateWeek == iWeek);

            Session.SetObjectAsJson("Reporters", model);
            return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Read_Report_by_Variance([DataSourceRequest] DataSourceRequest request, DateTime Date1, DateTime Date2)
        {
            //TBD, APPRO, MANAGER
            var model = Session.GetObjectFromJson<IEnumerable<BudgetBookCommitmentViewModel>>("Reporters");
            //model = GetBudgetBookCommitments(iYyear).Where(o => o.ApprovalStatus.Contains("APPRO") && o.AddedDateWeek == iWeek);
            model = GetBudgetBookCommitmentsVariance(Date1, Date2).Result;

            Session.SetObjectAsJson("Reporters", model);
            return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Read_CostBreakDown([DataSourceRequest] DataSourceRequest request, long parentId)
        {
            var model = Session.GetObjectFromJson<IEnumerable<ActivityDetailsViewModel>>("detls");
            model = GetCostBreakdownByCommitmentId(parentId); //.Where(o => o.BudgetBookCommitmentsID == parentId);

            Session.SetObjectAsJson("breakdown", model);
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult FilterMenuCustomization_CapexOpex()
        {
            return Json(repoCapexOpex.GetAll().Result.Select(e => e.CapexOpex).ToList());
        }

        public ActionResult FilterMenuCustomization_directAllocated()
        {
            return Json(repoCapexOpex.GetAll().Result.Select(e => e.CapexOpex).ToList());
        }

        public ActionResult FilterMenuCustomization_ActivityOwner()
        {
            return Json(repoCapexOpex.GetAll().Result.Select(e => e.CapexOpex).ToList());
        }

        public ActionResult FilterMenuCustomization_ActivityType()
        {
            return Json(repoActivityName.GetAll().Result.Select(e => e.ActivityName).ToList());
        }

        public ActionResult FilterMenuCustomization_ActivityCode()
        {
            return Json(repoActivityCode.GetAll().Result.Select(e => e.ActivityCodeDesc).ToList());
        }

        public ActionResult FilterMenuCustomization_Scope()
        {
            return Json(repoScope.GetAll().Result.Select(e => e.Purpose).ToList());
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        private async Task<IEnumerable<AppReportingViewModel>> GetReportings(int? iYear)
        {
            var summary = GetBudgetBookCommitments(iYear); //For current year
            decimal? TotalCapexApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)
                                                             && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Sum(o => o.Commitment);

            var TotalNoCapexApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)
                                                             && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Count();

            decimal? TotalCapexPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Sum(o => o.Commitment);
            var TotalNoCapexPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Count();

            decimal? TotalCapexRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                         && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                          || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                          || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) 
                                                          && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Sum(o => o.Commitment);

            var TotalNoCapexRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                      && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                       || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                       || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) 
                                                       && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Count();

            decimal? TotalCapexCommitment = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Sum(o => o.Commitment);
            var TotalNoOfCapexCommitments = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Count();

            decimal? TotalCapexSavings = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Capex)).Sum(o => o.Savings);

            decimal? TotalOpexApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)
                                                             && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Sum(o => o.Commitment);

            var TotalNoOpexApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)
                                                             && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Count();


            decimal? TotalOpexPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Sum(o => o.Commitment);
            var TotalNoOpexPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Count();

            decimal? TotalOpexRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                        && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                         || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                         || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) 
                                                         && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Sum(o => o.Commitment);

            var TotalNoOpexRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                                     && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                      || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                      || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval) 
                                                      && o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Count();

            decimal? TotalOpexCommitment = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Sum(o => o.Commitment);
            var TotalNoOfOpexCommitments = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Count();
            decimal? TotalOpexSavings = summary.Where(o => o.CapexOpex == OpsCaps.CapexOpexDesc(enuCapexOpex.Opex)).Sum(o => o.Savings);

            decimal? TotalApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)).Sum(o => o.Commitment);
            var TotalNoApproved = summary.Where(o => (o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                             || o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)).Count();

            decimal? TotalPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD).Sum(o => o.Commitment);
            var TotalNoPending = summary.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD).Count();

            decimal? TotalRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                               && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)).Sum(o => o.Commitment);
            var TotalNoRejected = summary.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.TBD 
                                            && (o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                             || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                             || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval)).Count();

            decimal? TotalCommitment = summary.Sum(o => o.Commitment);
            var TotalNoOfCommitments = summary.Count();
            decimal? TotalSavings = summary.Sum(o => o.Savings);

            //var result = GetBudgetBookCommitments(iYear).Select(o =>
            var result = summary.Select(o =>
            {
                return new AppReportingViewModel
                {
                    TotalCapexApproved = TotalCapexApproved,
                    TotalNoCapexApproved = TotalNoCapexApproved,
                    TotalCapexPending = TotalCapexPending,
                    TotalNoCapexPending = TotalNoCapexPending,
                    TotalCapexRejected = TotalCapexRejected,
                    TotalNoCapexRejected = TotalNoCapexRejected,
                    TotalCapexCommitment = TotalCapexCommitment,
                    TotalNoOfCapexCommitments = TotalNoOfCapexCommitments,
                    TotalCapexSavings = TotalCapexSavings,

                    TotalOpexApproved = TotalOpexApproved,
                    TotalNoOpexApproved = TotalNoOpexApproved,
                    TotalOpexPending = TotalOpexPending,
                    TotalNoOpexPending = TotalNoOpexPending,
                    TotalOpexRejected = TotalOpexRejected,
                    TotalNoOpexRejected = TotalNoOpexRejected,
                    TotalOpexCommitment = TotalOpexCommitment,
                    TotalNoOfOpexCommitments = TotalNoOfOpexCommitments,
                    TotalOpexSavings = TotalOpexSavings,

                    TotalApproved = TotalApproved,
                    TotalNoApproved = TotalNoApproved,
                    TotalPending = TotalPending,
                    TotalNoPending = TotalNoPending,
                    TotalRejected = TotalRejected,
                    TotalNoRejected = TotalNoRejected,
                    TotalCommitment = TotalCommitment,
                    TotalNoOfCommitments = TotalNoOfCommitments,
                    TotalSavings = TotalSavings,
                };
            }).ToList();
            return result;
        }

        public JsonResult GetCapexOpexReport(int? iYear)
        {
            int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;
            var result = GetReportings(iYyear).Result.FirstOrDefault();
            if (result == null) return Json(new object[] { new object() });
            else return Json(result);
        }

        //public JsonResult GetApprovalDecisions()
        //{
        //    List<ApprovalDecision> olist = new List<ApprovalDecision>();
        //    List<ApprovalDecision> list = (List<ApprovalDecision>)Enumerable.Empty<ApprovalDecision>();  //repoApprovalDecision.GetAll().Result.ToList();
        //    //List<ApprovalDecision> list = new SelectList(RolesManager.GetApprovalDecisions().OrderBy(o => o.Text), "Value", "Text");

        //    ViewBag.Roles = new SelectList(RolesManager.GetApprovalDecisions().OrderBy(o => o.Text), "Value", "Text");

        //    foreach (var t in list)
        //    {
        //        list.Select(o => new ApprovalDecision
        //        {
        //            ID = o.ID,
        //            Decision = o.Decision
        //        });

        //        olist.Add(list.FirstOrDefault());
        //    }

        //    list.AddRange(olist);

        //    list.Insert(0, new ApprovalDecision { ID = 0, Decision = "Select Decision..." });

        //    var result = new SelectList(list, "ID", "Decision");
        //    return Json(result);
        //}

        #endregion

        #region  ==================== Absence Management  ====================  

        public async Task<IActionResult> Router(long? iUserIdOwner, long? iUserIdAlternate, int? iRoleId)
        {
            //The JavaScript from Index, ActivityOwner, LineManager and Review views deliver the UserIDs of the Owner of the items 
            //and the selected Alternate, from the dropdownlist to be handed over to
            //Note: the handing over should be only the Pending items alone. The approved one
            var result = Enumerable.Empty<BudgetBookCommitmentViewModel>();
            var MyActivityCodes = Enumerable.Empty<ActivityCodeViewModel>();

            if (iRoleId == (int)enuRole.FocalPoint)
            {
                //result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Where(o => !o.ApprovalStatus.Contains("APPRO"));
                result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Result.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                                    || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                                    || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);

                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.FocalPointID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
            }
            else if (iRoleId == (int)enuRole.ActivityOwner)
            {
                //result = GetBudgetBookCommitmentsByActivityOwner(iUserIdOwner).Where(o => !o.ApprovalStatus.Contains("APPRO"));
                result = GetBudgetBookCommitmentsByActivityOwner(iUserIdOwner).Result.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                                    || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                                    || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.ActivityOwnerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                MyActivityCodes = GetActivityCodes().Where(o => o.ActivityOwnerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.ActivityOwnerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }
            }
            else if (iRoleId == (int)enuRole.LineManager)
            {
                //result = GetBudgetBookCommitmentsByLineManager(iUserIdOwner).Where(o => !o.ApprovalStatus.Contains("APPRO"));
                result = GetBudgetBookCommitmentsByLineManager(iUserIdOwner).Result.Where(o => o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedAsPresented
                                                                                   || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ApprovedWithModifications
                                                                                   || o.ApprovalID != (long)ApprovalDecisionType.enuApprovalDecision.ProvisionalApproval);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.LineManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                MyActivityCodes = GetActivityCodes().Where(o => o.LineManagerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.LineManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }
            }
            else if (iRoleId == (int)enuRole.AccountableManager)
            {
                result = GetBudgetBookCommitmentsByAccountableManager(iUserIdOwner).Result;
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.SponsorID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                //Note: SponsorID and AccountableManagerID are the same.
                MyActivityCodes = GetActivityCodes().Where(o => o.AccountableManagerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.AccountableManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }
            }

            RoutingMail(iUserIdOwner, iUserIdAlternate);
            return RedirectToAction("Routing");
        }

        public async Task<IActionResult> HandOverToAlternate(long? iUserIdOwner, long? iUserIdAlternate, int? iRoleId)
        {
            //The JavaScript from Index, ActivityOwner, LineManager and Review views deliver the UserIDs of the Owner of the items 
            //and the selected Alternate, from the dropdownlist to be handed over to
            //Note: the handing over should be only the Pending items alone. The approved one
            var result = Enumerable.Empty<BudgetBookCommitmentViewModel>();
            var MyActivityCodes = Enumerable.Empty<ActivityCodeViewModel>();

            if (iRoleId == (int)enuRole.FocalPoint)
            {
                //result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Where(o => o.ApprovalStatus.Contains("DRAFT"));
                result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Draft);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.FocalPointID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                RoutingMail(iUserIdOwner, iUserIdAlternate);
            }
            else if (iRoleId == (int)enuRole.Admin)
            {
                //Note: in case an Admin has things to handover. Admin can also function as a Focal Point
                //result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Where(o => o.ApprovalStatus.Contains("DRAFT"));
                result = GetBudgetBookCommitmentsByFocalPoint(iUserIdOwner).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.Draft);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.FocalPointID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                RoutingMail(iUserIdOwner, iUserIdAlternate);

                TempData["Message"] = "Successfully handed over to alternate.";
            }
            else if (iRoleId == (int)enuRole.ActivityOwner)
            {
                //result = GetBudgetBookCommitmentsByActivityOwner(iUserIdOwner).Where(o => o.ApprovalStatus.Contains("TBD"));
                result = GetBudgetBookCommitmentsByActivityOwner(iUserIdOwner).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.TBD);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.ActivityOwnerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                MyActivityCodes = GetActivityCodes().Where(o => o.ActivityOwnerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.ActivityOwnerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }

                RoutingMail(iUserIdOwner, iUserIdAlternate);
                return RedirectToAction("ActivityOwner");
            }
            else if (iRoleId == (int)enuRole.LineManager)
            {
                //result = GetBudgetBookCommitmentsByLineManager(iUserIdOwner).Where(o => o.ApprovalStatus.Contains("MANAGER") && o.ApproverID == null);
                result = GetBudgetBookCommitmentsByLineManager(iUserIdOwner).Result.Where(o => o.ApprovalID == (long)ApprovalDecisionType.enuApprovalDecision.LineManagerReview && o.ApproverID == null);
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.LineManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                MyActivityCodes = GetActivityCodes().Where(o => o.LineManagerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.LineManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }

                RoutingMail(iUserIdOwner, iUserIdAlternate);

                return RedirectToAction("LineManager");
            }
            else if (iRoleId == (int)enuRole.AccountableManager)
            {
                result = GetBudgetBookCommitmentsByAccountableManager(iUserIdOwner).Result;
                foreach (BudgetBookCommitmentViewModel o in result)
                {
                    BudgetBookCommitments entity = repoBudgetBookCommitments.GetById(o.ID).Result;
                    entity.SponsorID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Now.Date;

                    await repoBudgetBookCommitments.Update(entity);
                }
                //TODO: Update the ActivityCode Table to remap all activity codes accordingly
                //Note: SponsorID and AccountableManagerID are the same.
                MyActivityCodes = GetActivityCodes().Where(o => o.AccountableManagerID == iUserIdOwner);
                foreach (ActivityCodeViewModel o in MyActivityCodes)
                {
                    ActivityCode entity = repoActivityCode.GetById(o.ID).Result;
                    entity.AccountableManagerID = iUserIdAlternate;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoActivityCode.Update(entity);
                }

                RoutingMail(iUserIdOwner, iUserIdAlternate);

                return RedirectToAction("Review");
            }

            return RedirectToAction("Index");
        }

        public void RoutingMail(long? iUserIdOwner, long? iUserIdAlternate)
        {
            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            List<structUserMailIdx> mailTo = new List<structUserMailIdx>();
            List<structUserMailIdx> mailCopy = new List<structUserMailIdx>();

            AppUsers Owner = repoUsers.GetById(iUserIdOwner).Result;
            AppUsers Alternate = repoUsers.GetById(iUserIdAlternate).Result;

            IEnumerable<AppUsers> Admins = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.Admin);

            //TODO: Line Manager to Send mail to Focal Point, copy Activity owner and Accountable Manager, using MailKit
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Owner.FullName, Owner.UserMail));
            mailTo.Add(new structUserMailIdx(Alternate.FullName, Alternate.UserMail, Alternate.ID.ToString()));

            foreach (var admin in Admins)
            {
                mailCopy.Add(new structUserMailIdx(admin.FullName, admin.UserMail, admin.ID.ToString()));
            }

            mailCopy.Add(new structUserMailIdx(LoginUser.FullName, LoginUser.UserMail, LoginUser.ID.ToString()));

            var ccRouting = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "Router.html";

            structUserMailIdx mailFrom = new structUserMailIdx
            {
                m_sUserMail = LoginUser.UserMail,
                m_sUserName = LoginUser.FullName
            };

            var subject = "Commitment Control Absence Management";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(ccRouting))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                        Alternate.FullName,
                        Owner.FullName,
                        RolesManager.GetRoleByRole(Owner.RoleId),
                        GetBaseUrl(),
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
                        );
            _emailSender.SendEmailAsync(mailFrom, mailTo, mailCopy, subject, messageBody);

            //return true;
        }

        #endregion

        #region  ==================== Analysis Graphs  ====================

        public IActionResult Analysis()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Result.Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().Result.LastOrDefault() : result.FirstOrDefault();

            ViewData["commitmentsID"] = new SelectList((from s in repoBudgetBookCommitments.GetAll().Result.OrderBy(o => o.Comitmntno) select new { ID = s.ID, sTitle = s.Comitmntno + " - " + s.title }), "ID", "sTitle"); //Comitmntno
            int? iYear = DateTime.Today.Year;

            //model.CapexOpexList = repoCapexOpex.GetAll().Result.ToList();
            //model.CapexOpexList.Insert(0, new CXOX { ID = 0, CapexOpex = "Select..." });

            ViewBag.CapexOpex = new SelectList((from s in repoCapexOpex.GetAll().Result.OrderBy(o => o.CapexOpex) select new { ID = s.ID, Title = s.CapexOpex }), "ID", "Title");
            ViewBag.Activity = new SelectList((from s in repoActivityName.GetAll().Result.OrderBy(o => o.ActivityName) select new { ID = s.ID, Title = s.ActivityName }), "ID", "Title");
            ViewBag.ActivityCode = new SelectList((from s in repoActivityCode.GetAll().Result.OrderBy(o => o.ActivityCodeDesc) select new { ID = s.ID, Title = s.ActivityCodeDesc }), "ID", "Title");

            return View(model);
        }

        #endregion
    }
}