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
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.IO;
using OfficeOpenXml;
using Web.BongaCC.Codes;

namespace Web.BongaCC.Controllers
{
    public class BudgetUploaderController : Controller
    {
        private readonly IRepository<BudgetUploader> repo;
        private readonly IRepository<BudgetUploaderTransformed> repoTransformed;

        private readonly IRepository<BudgetBasis> repoBudgetBasis;
        private readonly IRepository<Activity> repoActivity;
        private readonly IRepository<ActivityType> repoActivityName;
        private readonly IRepository<Scope> repoScope;
        private readonly IRepository<Contract> repoContract;
        private readonly IRepository<UapCode> repoUapCode;
        private readonly IRepository<UapRollUpCode> repoUapRollUpCode;
        private readonly IRepository<ActivityCode> repoActivityCode;
        private readonly IRepository<CXOX> repoCapexOpex;
        private readonly IRepository<AppUsers> repoUsers;
        private readonly IRepository<WBS> repoWBS;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        private readonly ISession _session;
        public ISession Session { get { return _session; } }

        public BudgetUploaderController(IRepository<BudgetUploader> repo, IRepository<BudgetUploaderTransformed> repoTransformed, IRepository<BudgetBasis> repoBudgetBasis, IRepository<Activity> repoActivity,
            IRepository<ActivityType> repoActivityName, IRepository<Scope> repoScope, IRepository<Contract> repoContract,
            IRepository<UapCode> repoUapCode, IRepository<UapRollUpCode> repoUapRollUpCode, IRepository<ActivityCode> repoActivityCode,
            IRepository<CXOX> repoCapexOpex, IRepository<AppUsers> repoUsers, IRepository<WBS> repoWBS,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this.repo = repo;
            this.repoTransformed = repoTransformed;
            this.repoWBS = repoWBS;
            this.repoUsers = repoUsers;
            this.repoBudgetBasis = repoBudgetBasis; this.repoActivity = repoActivity;
            this.repoActivityName = repoActivityName; this.repoScope = repoScope; this.repoContract = repoContract;
            this.repoUapCode = repoUapCode; this.repoUapRollUpCode = repoUapRollUpCode; this.repoActivityCode = repoActivityCode;
            this.repoCapexOpex = repoCapexOpex;
            _session = httpContextAccessor.HttpContext.Session;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            BudgetUploaderViewModel model = new BudgetUploaderViewModel();

            return View(model);
        }

        public IActionResult Upload(IFormCollection formCollection)
        {
            var budgetBook = new List<BudgetUploader>();
            if (Request != null)
            {
                IFormFile file = Request.Form.Files["UploadedFile"];
                var contentType = (!string.IsNullOrEmpty(file.FileName)) ? GetContentType(file.FileName) : "";
                var memoryStream = new MemoryStream();

                if ((file != null) && (contentType.ToLower() != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                {
                    TempData["Message"] = "Invalid file selected!!! \\nSelect an excel file for CC Bulkupload.";
                    return View("Index");
                }
                else if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    //Delete all data before carrying out any update.
                    repo.DeleteRange(repo.GetAll().Result);
                    
                    var data = file.CopyToAsync(memoryStream);
                    memoryStream.ToArray();

                    using (var package = new ExcelPackage(memoryStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var bbook = new BudgetUploader();
                            //Note: the fist column on the Excelsheet should be S/No.                                                                                //1.  S/No
                            bbook.ActivityType = (workSheet.Cells[rowIterator, 2].Value != null) ? workSheet.Cells[rowIterator, 2].Value.ToString() : "";            //2.  Capex Opex
                            bbook.DirectAllocated = (workSheet.Cells[rowIterator, 3].Value != null) ? workSheet.Cells[rowIterator, 3].Value.ToString() : "";         //3.  Direct or Allocated
                            bbook.UapCode = (workSheet.Cells[rowIterator, 4].Value != null) ? workSheet.Cells[rowIterator, 4].Value.ToString() : "";                 //4.  UapCode
                            bbook.UapRollUpCode = (workSheet.Cells[rowIterator, 5].Value != null) ? workSheet.Cells[rowIterator, 5].Value.ToString() : "";           //5.  UapRollUpCode
                            bbook.ActivityName = (workSheet.Cells[rowIterator, 6].Value != null) ? workSheet.Cells[rowIterator, 6].Value.ToString() : "";            //6.  ActivityName
                            bbook.ActivityCode = (workSheet.Cells[rowIterator, 7].Value != null) ? workSheet.Cells[rowIterator, 7].Value.ToString() : "";            //7.  ActivityCode
                            bbook.CostCenter = (workSheet.Cells[rowIterator, 8].Value != null) ? workSheet.Cells[rowIterator, 8].Value.ToString() : "";              //8.  CostCenter(WBS)
                            bbook.Activity = (workSheet.Cells[rowIterator, 9].Value != null) ? workSheet.Cells[rowIterator, 9].Value.ToString() : "";                //9.  Activity                            
                            bbook.ActivityOwner = (workSheet.Cells[rowIterator, 10].Value != null) ? workSheet.Cells[rowIterator, 10].Value.ToString() : "";        //10. ActivityOwner
                            bbook.LineManager = (workSheet.Cells[rowIterator, 11].Value != null) ? workSheet.Cells[rowIterator, 11].Value.ToString() : "";          //11.  LineManager (Please, emailaddress)
                            bbook.AccountableManager = (workSheet.Cells[rowIterator, 12].Value != null) ? workSheet.Cells[rowIterator, 12].Value.ToString() : "";   //12. AccountableManager
                            bbook.ScopePurpose = (workSheet.Cells[rowIterator, 13].Value != null) ? workSheet.Cells[rowIterator, 13].Value.ToString() : "";         //13. ScopePurpose
                            bbook.Contract = (workSheet.Cells[rowIterator, 14].Value != null) ? workSheet.Cells[rowIterator, 14].Value.ToString() : "";             //14. Contract
                            bbook.Budgetbasis = (workSheet.Cells[rowIterator, 15].Value != null) ? workSheet.Cells[rowIterator, 15].Value.ToString() : "";          //15. Budgetbasis
                            bbook.OPYearBudget = (workSheet.Cells[rowIterator, 16].Value != null) ? Convert.ToInt32(workSheet.Cells[rowIterator, 16].Value) : 0.0M; //16. NAPIMS FDollar
                            bbook.YYear = DateTime.Today.Year;                                                                                                      //Current year

                            budgetBook.Add(bbook);
                        }
                    }
                }
                else
                {
                    TempData["Message"] = "File is empty!!! \\nSelect CCP Budget Book Bulk Upload Excel Format, designed for Data upload, with Updated Budgetbook data.";
                    return View("Index");
                }
            }
            else
            {
                TempData["Message"] = "Please Select the CCP Budget Book Bulk Upload Excel Format, designed for Data upload.";
                return View("Index");
            }



            foreach (var item in budgetBook)
            {
                repo.Insert(item);
            }

            return View("Index");
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

        private IEnumerable<BudgetUploaderViewModel> LstGetBudgets()
        {
            int? iYyear = DateTime.Today.Year;

            var result = repo.GetAll().Result.Where(o => o.YYear == iYyear).OrderBy(o => o.ID).ToList().Select(entity =>
                {
                    return new BudgetUploaderViewModel
                    {
                        ID = entity.ID,
                        ActivityType = entity.ActivityType,
                        DirectAllocated = entity.DirectAllocated,
                        UapCode = entity.UapCode,
                        UapRollUpCode = entity.UapRollUpCode,
                        ActivityName = entity.ActivityName,

                        ActivityCode = entity.ActivityCode,
                        LineManager = entity.LineManager,

                        CostCenter = entity.CostCenter,
                        Activity = entity.Activity,
                        ActivityOwner = entity.ActivityOwner,
                        AccountableManager = entity.AccountableManager,
                        ScopePurpose = entity.ScopePurpose,
                        Contract = entity.Contract,
                        Budgetbasis = entity.Budgetbasis,
                        OPYearBudget = entity.OPYearBudget,
                        YYear = entity.YYear,
                    };
                }).ToList();
            return result;
        }

        public IActionResult ValidateBudgetBook()
        {
            IEnumerable<BudgetUploaderViewModel> rawDataResult = LstGetBudgets();

            var actitivyTypes = rawDataResult.Select(o => o.ActivityType.ToUpper().Trim()).Distinct();
            var actitivyTypes2 = repoCapexOpex.GetAll().Result.Select(o => o.CapexOpex.ToUpper().Trim()).ToList();
            var actitivyTypes3 = actitivyTypes.Except(actitivyTypes2);

            var uapcodes = rawDataResult.Select(o => o.UapCode.ToUpper().Trim()).Distinct();
            var uapcodes2 = repoUapCode.GetAll().Result.Select(o => o.UapCodeDesc.ToUpper().Trim()).ToList();
            var uapcodes3 = uapcodes.Except(uapcodes2);

            var uaprollupcodes = rawDataResult.Select(o => o.UapRollUpCode.ToUpper().Trim()).Distinct();
            var uaprollupcodes2 = repoUapRollUpCode.GetAll().Result.Select(o => o.UapRollUpCodeDesc.ToUpper().Trim()).ToList();
            var uaprollupcodes3 = uaprollupcodes.Except(uaprollupcodes2);

            var actitivyNames = rawDataResult.Select(o => o.ActivityName.ToUpper().Trim()).Distinct();
            var actitivyNames2 = repoActivityName.GetAll().Result.Select(o => o.ActivityName.ToUpper().Trim()).ToList();
            var actitivyNames3 = actitivyNames.Except(actitivyNames2);

            var actitivyCodes = rawDataResult.Select(o => o.ActivityCode.ToUpper().Trim()).Distinct();
            var actitivyCodes2 = repoActivityCode.GetAll().Result.Select(o => o.ActivityCodeDesc.ToUpper().Trim()).ToList();
            var actitivyCodes3 = actitivyCodes.Except(actitivyCodes2);

            var CostCentre = rawDataResult.Select(o => o.CostCenter.ToUpper().Trim()).Distinct();
            var CostCentre2 = repoWBS.GetAll().Result.Select(o => o.CostObjects.ToUpper().Trim()).ToList();
            var CostCentre3 = CostCentre.Except(CostCentre2);

            var actitivys = rawDataResult.Select(o => o.Activity.ToUpper().Trim()).Distinct();
            var actitivys2 = repoActivity.GetAll().Result.Select(o => o.Description.ToUpper().Trim()).ToList();
            var actitivys3 = actitivys.Except(actitivys2);

            var actitivyOwners = rawDataResult.Select(o => o.ActivityOwner.ToUpper().Trim()).Distinct();
            var actitivyOwners2 = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).Select(o => o.UserMail.ToUpper().Trim()).ToList();
            var actitivyOwners3 = actitivyOwners.Except(actitivyOwners2);

            var LineManager = rawDataResult.Select(o => o.LineManager.ToUpper().Trim()).Distinct();
            var LineManager2 = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).Select(o => o.UserMail.ToUpper().Trim()).ToList();
            var LineManager3 = LineManager.Except(LineManager2);

            var sponsors = rawDataResult.Select(o => o.AccountableManager.ToUpper().Trim()).Distinct();
            var sponsors2 = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).Select(o => o.UserMail.ToUpper().Trim()).ToList();
            var sponsors3 = sponsors.Except(sponsors2);

            var Scopes = rawDataResult.Select(o => o.ScopePurpose.ToUpper().Trim()).Distinct();
            var Scopes2 = repoScope.GetAll().Result.Select(o => o.Purpose.ToUpper().Trim()).ToList();
            var Scopes3 = Scopes.Except(Scopes2);

            var contracts = rawDataResult.Select(o => o.Contract.ToUpper().Trim()).Distinct();
            var contracts2 = repoContract.GetAll().Result.Select(o => o.ContractName.ToUpper().Trim()).ToList();
            var contracts3 = contracts.Except(contracts2);

            var budgetBases = rawDataResult.Select(o => o.Budgetbasis.ToUpper().Trim()).Distinct();
            var budgetBases2 = repoBudgetBasis.GetAll().Result.Select(o => o.BudgetBase.ToUpper().Trim()).ToList();
            var budgetBases3 = budgetBases.Except(budgetBases2);


            int i = 1;
            var header = "";
            header += "<table border='1' borderColor='silver' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri;'> ";
            header += "<thead>";
            header += "<th>S/No</th>";
            header += "<th>@PlaceHolder</th>";
            header += "</thead>";

            var sActivityTypes = "";
            sActivityTypes += header.Replace("@PlaceHolder", "Activity Types");
            foreach (string model in actitivyTypes3)
            {
                sActivityTypes += "<tr>";
                sActivityTypes += "<td>" + i++ + "</td>";
                sActivityTypes += "<td>" + model + "</td>";
                sActivityTypes += "</tr>";
            }

            sActivityTypes += "</table>";
            ViewBag.ActivityTypes = sActivityTypes;

            i = 1;
            var sUapCodes = "";
            sUapCodes += header.Replace("@PlaceHolder", "UAP Codes");
            foreach (string model in uapcodes3)
            {
                sUapCodes += "<tr>";
                sUapCodes += "<td>" + i++ + "</td>";
                sUapCodes += "<td>" + model + "</td>";
                sUapCodes += "</tr>";
            }

            sUapCodes += "</table>";
            ViewBag.UapCodes = sUapCodes;

            i = 1;
            var sUapRollUpCodes = "";
            sUapRollUpCodes += header.Replace("@PlaceHolder", "UAP Roll up codes");
            foreach (string model in uaprollupcodes3)
            {
                sUapRollUpCodes += "<tr>";
                sUapRollUpCodes += "<td>" + i++ + "</td>";
                sUapRollUpCodes += "<td>" + model + "</td>";
                sUapRollUpCodes += "</tr>";
            }

            sUapRollUpCodes += "</table>";
            ViewBag.UapRollUpCodes = sUapRollUpCodes;

            i = 1;
            var sActivityNames = "";
            sActivityNames += header.Replace("@PlaceHolder", "Activity Names");
            foreach (string model in actitivyNames3)
            {
                sActivityNames += "<tr>";
                sActivityNames += "<td>" + i++ + "</td>";
                sActivityNames += "<td>" + model + "</td>";
                sActivityNames += "</tr>";
            }

            sActivityNames += "</table>";
            ViewBag.ActivityNames = sActivityNames;

            i = 1;
            var sActivityCodees = "";
            sActivityCodees += header.Replace("@PlaceHolder", "Activity Codes");
            foreach (string model in actitivyCodes3)
            {
                sActivityCodees += "<tr>";
                sActivityCodees += "<td>" + i++ + "</td>";
                sActivityCodees += "<td>" + model + "</td>";
                sActivityCodees += "</tr>";
            }

            sActivityCodees += "</table>";
            ViewBag.ActivityCodees = sActivityCodees;

            i = 1;
            var sCostCenter = "";
            sCostCenter += header.Replace("@PlaceHolder", "Cost CEntre");
            foreach (string model in CostCentre3)
            {
                sCostCenter += "<tr>";
                sCostCenter += "<td>" + i++ + "</td>";
                sCostCenter += "<td>" + model + "</td>";
                sCostCenter += "</tr>";
            }
            sCostCenter += "</table>";
            ViewBag.CostCenter = sCostCenter;

            i = 1;
            var sActivity = "";
            sActivity += header.Replace("@PlaceHolder", "Activities");
            foreach (string model in actitivys3)
            {
                sActivity += "<tr>";
                sActivity += "<td>" + i++ + "</td>";
                sActivity += "<td>" + model + "</td>";
                sActivity += "</tr>";
            }
            sActivity += "</table>";
            ViewBag.Activity = sActivity;

            i = 1;
            var sActivityOwner = "";
            sActivityOwner += header.Replace("@PlaceHolder", "Activity Owners");
            foreach (string model in actitivyOwners3)
            {
                sActivityOwner += "<tr>";
                sActivityOwner += "<td>" + i++ + "</td>";
                sActivityOwner += "<td>" + model + "</td>";
                sActivityOwner += "</tr>";
            }
            sActivityOwner += "</table>";
            ViewBag.ActivityOwner = sActivityOwner;

            i = 1;
            var sLineManager = "";
            sLineManager += header.Replace("@PlaceHolder", "Line Managers");
            foreach (string model in LineManager3)
            {
                sLineManager += "<tr>";
                sLineManager += "<td>" + i++ + "</td>";
                sLineManager += "<td>" + model + "</td>";
                sLineManager += "</tr>";
            }
            sLineManager += "</table>";
            ViewBag.LineManager = sLineManager;

            i = 1;
            var sSponsors = "";
            sSponsors += header.Replace("@PlaceHolder", "Accountable Managers");
            foreach (string model in sponsors3)
            {
                sSponsors += "<tr>";
                sSponsors += "<td>" + i++ + "</td>";
                sSponsors += "<td>" + model + "</td>";
                sSponsors += "</tr>";
            }
            sSponsors += "</table>";
            ViewBag.Sponsors = sSponsors;

            i = 1;
            var sScopes = "";
            sScopes += header.Replace("@PlaceHolder", "Scopes");
            foreach (string model in Scopes3)
            {
                sScopes += "<tr>";
                sScopes += "<td>" + i++ + "</td>";
                sScopes += "<td>" + model + "</td>";
                sScopes += "</tr>";
            }
            sScopes += "</table>";
            ViewBag.Scopes = sScopes;

            i = 1;
            var scontracts = "";
            scontracts += header.Replace("@PlaceHolder", "Contracts");
            foreach (string model in contracts3)
            {
                scontracts += "<tr>";
                scontracts += "<td>" + i++ + "</td>";
                scontracts += "<td>" + model + "</td>";
                scontracts += "</tr>";
            }
            scontracts += "</table>";
            ViewBag.contracts = scontracts;

            i = 1;
            var sbudgetBases = "";
            sbudgetBases += header.Replace("@PlaceHolder", "Budget Bases");
            foreach (string model in budgetBases3)
            {
                sbudgetBases += "<tr>";
                sbudgetBases += "<td>" + i++ + "</td>";
                sbudgetBases += "<td>" + model + "</td>";
                sbudgetBases += "</tr>";
            }
            sbudgetBases += "</table>";
            ViewBag.budgetBases = sbudgetBases;

            return PartialView("_ShowValidateErrors");
        }

        public async Task<IActionResult> CommitToRespectiveTables()
        {
            IEnumerable<BudgetUploaderViewModel> result = LstGetBudgets();

            var actitivyTypes = result.Select(o => o.ActivityType.ToUpper().Trim()).Distinct();
            var actitivyTypes2 = repoCapexOpex.GetAll().Result.Select(o => o.CapexOpex.ToUpper().Trim()).ToList();
            var actitivyTypes3 = actitivyTypes.Except(actitivyTypes2);
            CXOX entityCO = new CXOX();
            foreach (var model in actitivyTypes3)
            {
                entityCO.ID = null;
                entityCO.AddedDate = DateTime.Today.Date;
                entityCO.ModifiedDate = DateTime.Today.Date;
                entityCO.CapexOpex = model;

                await repoCapexOpex.Insert(entityCO);
            }

            var uapcodes = result.Select(o => o.UapCode.ToUpper().Trim()).Distinct();
            var uapcodes2 = repoUapCode.GetAll().Result.Select(o => o.UapCodeDesc.ToUpper().Trim()).ToList();
            var uapcodes3 = uapcodes.Except(uapcodes2);
            UapCode entityUapCode = new UapCode();
            foreach (var model in uapcodes3)
            {
                entityUapCode.ID = null;
                entityUapCode.AddedDate = DateTime.Today.Date;
                entityUapCode.ModifiedDate = DateTime.Today.Date;
                entityUapCode.UapCodeDesc = model;

                await repoUapCode.Insert(entityUapCode);
            }

            var uaprollupcodes = result.Select(o => o.UapRollUpCode.ToUpper().Trim()).Distinct();
            var uaprollupcodes2 = repoUapRollUpCode.GetAll().Result.Select(o => o.UapRollUpCodeDesc.ToUpper().Trim()).ToList();
            var uaprollupcodes3 = uaprollupcodes.Except(uaprollupcodes2);
            UapRollUpCode entityUapRollUpCode = new UapRollUpCode();
            foreach (var model in uaprollupcodes3)
            {
                entityUapRollUpCode.ID = null;
                entityUapRollUpCode.AddedDate = DateTime.Today.Date;
                entityUapRollUpCode.ModifiedDate = DateTime.Today.Date;
                entityUapRollUpCode.UapRollUpCodeDesc = model;

                await repoUapRollUpCode.Insert(entityUapRollUpCode);
            }

            var actitivyNames = result.Select(o => o.ActivityName.ToUpper().Trim()).Distinct();
            var actitivyNames2 = repoActivityName.GetAll().Result.Select(o => o.ActivityName.ToUpper().Trim()).ToList();
            var actitivyNames3 = actitivyNames.Except(actitivyNames2);
            ActivityType entityActivityType = new ActivityType();
            foreach (var model in actitivyNames3)
            {
                entityActivityType.ID = null;
                entityActivityType.AddedDate = DateTime.Today.Date;
                entityActivityType.ModifiedDate = DateTime.Today.Date;
                entityActivityType.ActivityName = model;

                await repoActivityName.Insert(entityActivityType);
            }



            var ActivityOwners = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList();
            var LineManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).ToList();
            var Sponsors = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList();

            var actitivyCodes = result.Select(o => o.ActivityCode.ToUpper().Trim()).Distinct();
            var actitivyCodes2 = repoActivityCode.GetAll().Result.Select(o => o.ActivityCodeDesc.ToUpper().Trim()).ToList();
            var actitivyCodes3 = actitivyCodes.Except(actitivyCodes2);
            ActivityCode entityActivityCode = new ActivityCode();
            foreach (var model in actitivyCodes3)
            {
                //Foreach activity code, derive the Activity Owenr, Line Manager and Accountable Manager
                var entito = result.Where(t => t.ActivityCode.ToUpper().Trim() == model).FirstOrDefault();              
                var tActivityOwner = ActivityOwners.Where(c => c.UserMail.ToUpper().Trim().Contains(entito.ActivityOwner.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var ActivityOwner = ((ActivityOwners.Count != 0) && (tActivityOwner != null)) ? tActivityOwner.ID.ToString() : entito.ActivityOwner;

                var tLineManager = LineManagers.Where(c => c.UserMail.ToUpper().Trim().Contains(entito.LineManager.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var LineManager = ((LineManagers.Count != 0) && (tLineManager != null)) ? tLineManager.ID.ToString() : entito.LineManager;

                var tSponsor = Sponsors.Where(c => c.UserMail.ToUpper().Trim().Contains(entito.AccountableManager.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var Sponsor = ((Sponsors.Count != 0) && (tSponsor != null)) ? tSponsor.ID.ToString() : entito.AccountableManager;

                entityActivityCode.ID = null;
                entityActivityCode.AddedDate = DateTime.Today.Date;
                entityActivityCode.ModifiedDate = DateTime.Today.Date;
                entityActivityCode.ActivityCodeDesc = model;

                entityActivityCode.ActivityOwnerID = long.Parse(ActivityOwner);
                entityActivityCode.LineManagerID = long.Parse(LineManager);
                entityActivityCode.AccountableManagerID = long.Parse(Sponsor);

                await repoActivityCode.Insert(entityActivityCode);
            }

            var CostCentre = result.Select(o => o.CostCenter.ToUpper().Trim()).Distinct();
            var CostCentre2 = repoWBS.GetAll().Result.Select(o => o.CostObjects.ToUpper().Trim()).ToList();
            var CostCentre3 = CostCentre.Except(CostCentre2);
            WBS entityWBS = new WBS();
            foreach (var model in CostCentre3)
            {
                entityWBS.ID = null;
                entityWBS.AddedDate = DateTime.Today.Date;
                entityWBS.ModifiedDate = DateTime.Today.Date;
                entityWBS.CostObjects = model;

                await repoWBS.Insert(entityWBS);
            }

            var actitivys = result.Select(o => o.Activity.ToUpper().Trim()).Distinct();
            var actitivys2 = repoActivity.GetAll().Result.Select(o => o.Description.ToUpper().Trim()).ToList();
            var actitivys3 = actitivys.Except(actitivys2);
            Activity entity = new Activity();
            foreach (var model in actitivys3)
            {
                entity.ID = null;
                entity.AddedDate = DateTime.Today.Date;
                entity.ModifiedDate = DateTime.Today.Date;
                entity.Description = model;

                await repoActivity.Insert(entity);
            }

            var Scopes = result.Select(o => o.ScopePurpose.ToUpper().Trim()).Distinct();
            var Scopes2 = repoScope.GetAll().Result.Select(o => o.Purpose.ToUpper().Trim()).ToList();
            var Scopes3 = Scopes.Except(Scopes2);
            Scope entityScope = new Scope();
            foreach (var model in Scopes3)
            {
                entityScope.ID = null;
                entityScope.AddedDate = DateTime.Today.Date;
                entityScope.ModifiedDate = DateTime.Today.Date;
                entityScope.Purpose = model;

                await repoScope.Insert(entityScope);
            }

            var contracts = result.Select(o => o.Contract.ToUpper().Trim()).Distinct();
            var contracts2 = repoContract.GetAll().Result.Select(o => o.ContractName.ToUpper().Trim()).ToList();
            var contracts3 = contracts.Except(contracts2);
            Contract entityContract = new Contract();
            foreach (var model in contracts3)
            {
                entityContract.ID = null;
                entityContract.AddedDate = DateTime.Today.Date;
                entityContract.ModifiedDate = DateTime.Today.Date;
                entityContract.ContractName = model;

                await repoContract.Insert(entityContract);
            }

            var budgetBases = result.Select(o => o.Budgetbasis.ToUpper().Trim()).Distinct();
            var budgetBases2 = repoBudgetBasis.GetAll().Result.Select(o => o.BudgetBase.ToUpper().Trim()).ToList();
            var budgetBases3 = budgetBases.Except(budgetBases2);
            BudgetBasis entityBudgetBasis = new BudgetBasis();
            foreach (var model in budgetBases3)
            {
                entityBudgetBasis.ID = null;
                entityBudgetBasis.AddedDate = DateTime.Today.Date;
                entityBudgetBasis.ModifiedDate = DateTime.Today.Date;
                entityBudgetBasis.BudgetBase = model;

                await repoBudgetBasis.Insert(entityBudgetBasis);
            }


            TempData["Message"] = "Process was succesful!!! Click Transform Data for Upload to continue.";

            return RedirectToAction("Index");
        }

        public IActionResult LoadData()
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
                var customerData = LstGetBudgets(); // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Activity.ToUpper().Contains(searchValue)); //Search  
                //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Activity.Contains(searchValue)); //Search  


                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> InsertTranformedData()
        {
            //Delete all data before carrying out any update.
            await repoTransformed.DeleteRange(repoTransformed.GetAll().Result);

            IEnumerable<BudgetUploaderViewModel> result = GetTransformedBudgetBook();

            foreach (BudgetUploaderViewModel o in result)
            {
                bool isNew = !o.ID.HasValue;
                BudgetUploaderTransformed entity = isNew ? new BudgetUploaderTransformed { AddedDate = DateTime.Today.Date, ModifiedDate = DateTime.Today.Date } : await repoTransformed.GetById(o.ID);
                entity.ID = o.ID;
                entity.ActivityType = o.ActivityType;
                entity.DirectAllocated = o.DirectAllocated;
                entity.UapCode = o.UapCode;
                entity.UapRollUpCode = o.UapRollUpCode;
                entity.ActivityName = o.ActivityName;

                entity.ActivityCode = o.ActivityCode;
                entity.LineManager = o.LineManager;

                entity.CostCenter = o.CostCenter;
                entity.Activity = o.Activity;
                entity.ActivityOwner = o.ActivityOwner;
                entity.AccountableManager = o.AccountableManager;
                entity.ScopePurpose = o.ScopePurpose;
                entity.Contract = o.Contract;
                entity.Budgetbasis = o.Budgetbasis;
                entity.OPYearBudget = o.OPYearBudget;
                entity.YYear = DateTime.Today.Year;

                if (isNew)
                {
                    await repoTransformed.Insert(entity);
                }
                else
                {
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoTransformed.Update(entity);
                }
            }
            return RedirectToAction("Index", "BudgetUploaderTransformed");
        }

        public async Task<IActionResult> AddEdit(BudgetUploaderViewModel model)
        {
            bool isNew = !model.ID.HasValue;
            BudgetUploader entity = isNew ? new BudgetUploader { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
            entity.ID = model.ID;
            entity.ActivityType = model.ActivityType;
            entity.DirectAllocated = model.DirectAllocated;
            entity.UapCode = model.UapCode;
            entity.UapRollUpCode = model.UapRollUpCode;
            entity.ActivityName = model.ActivityName;

            entity.ActivityCode = model.ActivityCode;
            entity.LineManager = model.LineManager;

            entity.CostCenter = model.CostCenter;
            entity.Activity = model.Activity;
            entity.ActivityOwner = model.ActivityOwner;
            entity.AccountableManager = model.AccountableManager;
            entity.ScopePurpose = model.ScopePurpose;
            entity.Contract = model.Contract;
            entity.Budgetbasis = model.Budgetbasis;
            entity.OPYearBudget = model.OPYearBudget;

            if (isNew)
            {
                await repo.Insert(entity);
            }
            else
            {
                entity.ModifiedDate = DateTime.Today.Date;
                await repo.Update(entity);
            }
            return RedirectToAction("Index");
        }

        private IEnumerable<BudgetUploaderViewModel> GetTransformedBudgetBook()
        {
            var CapexOpexs = repoCapexOpex.GetAll().Result.ToList();
            var UapCodes = repoUapCode.GetAll().Result.ToList();
            var UapRollUpCodes = repoUapRollUpCode.GetAll().Result.ToList();
            var ActivityNames = repoActivityName.GetAll().Result.ToList();
            var ActivityCodes = repoActivityCode.GetAll().Result.ToList();
            var WBSes = repoWBS.GetAll().Result.ToList();
            var Activities = repoActivity.GetAll().Result.ToList();
            var ActivityOwners = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList();
            var LineManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).ToList();
            var Sponsors = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList();
            var Scopes = repoScope.GetAll().Result.ToList();
            var Contracts = repoContract.GetAll().Result.ToList();
            var BudgetBases = repoBudgetBasis.GetAll().Result.ToList();
            //var Uploader = repo.GetAll().Result.ToList();

            string UserMail = Apps.getFullEmail(User.Identity.Name);
            AppUsers LoginUser = repoUsers.GetAll().Result.Where(c => c.UserMail.ToLower().Trim() == UserMail.ToLower().Trim()).FirstOrDefault();

            //var result = repo.GetAll().Result.ToList().Select(entity =>
            var result = repo.GetAll().Result.ToList().Select(entity =>
            {
                var tCapexOpex = CapexOpexs.FirstOrDefault(c => entity.ActivityType.ToUpper().Trim() == c.CapexOpex.ToUpper().Trim());
                var CapexOpex = (tCapexOpex != null) ? tCapexOpex.ID.ToString() : entity.ActivityType;

                var tUapCode = UapCodes.FirstOrDefault(c => entity.UapCode.ToUpper().Trim() == c.UapCodeDesc.ToUpper().Trim());
                var UapCode = (tUapCode != null) ? tUapCode.ID.ToString() : entity.UapCode;

                var tUaprollUpcode = UapRollUpCodes.FirstOrDefault(c => entity.UapRollUpCode.ToUpper().Trim() == c.UapRollUpCodeDesc.ToUpper().Trim());
                var UaprollUpcode = (tUaprollUpcode != null) ? tUaprollUpcode.ID.ToString() : entity.UapRollUpCode;

                var tActivityName = ActivityNames.FirstOrDefault(c => entity.ActivityName.ToUpper().Trim() == c.ActivityName.ToUpper().Trim());
                var ActivityName = (tActivityName != null) ? tActivityName.ID.ToString() : entity.ActivityName;

                var tActivityCode = ActivityCodes.FirstOrDefault(c => entity.ActivityCode.ToUpper().Trim() == c.ActivityCodeDesc.ToUpper().Trim());
                var ActivityCode = (tActivityCode != null) ? tActivityCode.ID.ToString() : entity.ActivityCode;

                var twbs = WBSes.FirstOrDefault(c => entity.CostCenter.ToUpper().Trim() == c.CostObjects.ToUpper().Trim());
                var wbs = (twbs != null) ? twbs.ID.ToString() : entity.CostCenter;

                var tActivity = Activities.FirstOrDefault(c => entity.Activity.ToUpper().Trim() == c.Description.ToUpper().Trim());
                var Activity = (tActivity != null) ? tActivity.ID.ToString() : entity.Activity;

                var tScope = Scopes.FirstOrDefault(c => entity.ScopePurpose.ToUpper().Trim() == c.Purpose.ToUpper().Trim());
                var Scope = (tScope != null) ? tScope.ID.ToString() : entity.ScopePurpose;

                var tContract = Contracts.FirstOrDefault(c => entity.Contract.ToUpper().Trim() == c.ContractName.ToUpper().Trim());
                var Contract = (tContract != null) ? tContract.ID.ToString() : entity.Contract;

                var tBudgetBasis = BudgetBases.FirstOrDefault(c => entity.Budgetbasis.ToUpper().Trim() == c.BudgetBase.ToUpper().Trim());
                var BudgetBasis = (tBudgetBasis != null) ? tBudgetBasis.ID.ToString() : entity.Budgetbasis;


                //var Sponsor = Sponsors.Count != 0 ? Sponsors.FirstOrDefault(c => entity.AccountableManager.ToUpper().Trim() == c.FullName.ToUpper().Trim()) : new AppUsers();
                var tActivityOwner = ActivityOwners.Where(c => c.UserMail.ToUpper().Trim().Contains(entity.ActivityOwner.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var ActivityOwner = ((ActivityOwners.Count != 0) && (tActivityOwner != null)) ? tActivityOwner.ID.ToString() : entity.ActivityOwner;

                var tLineManager = LineManagers.Where(c => c.UserMail.ToUpper().Trim().Contains(entity.LineManager.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var LineManager = ((LineManagers.Count != 0) && (tLineManager != null)) ? tLineManager.ID.ToString() : entity.LineManager;

                var tSponsor = Sponsors.Where(c => c.UserMail.ToUpper().Trim().Contains(entity.AccountableManager.Substring(0, 4).ToUpper().Trim())).FirstOrDefault();
                var Sponsor = ((Sponsors.Count != 0) && (tSponsor != null)) ? tSponsor.ID.ToString() : entity.AccountableManager;

                return new BudgetUploaderViewModel
                {
                    //ID = entity.ID,
                    ActivityType = CapexOpex,
                    DirectAllocated = DirectAllocated.IntDirectAllocated(entity.DirectAllocated).ToString(),
                    UapCode = UapCode,
                    UapRollUpCode = UaprollUpcode,
                    ActivityName = ActivityName,
                    ActivityCode = ActivityCode,
                    CostCenter = wbs,
                    Activity = Activity,
                    ActivityOwner = ActivityOwner,
                    LineManager = LineManager,
                    AccountableManager = Sponsor,
                    ScopePurpose = Scope,
                    Contract = Contract,
                    Budgetbasis = BudgetBasis,
                    OPYearBudget = entity.OPYearBudget,
                    YYear = DateTime.Today.Year,
                };
            }).ToList();
            return result;
        }

        // GET: ActivityCodes/Edit/5
        //public IActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    BudgetUploaderViewModel model = GetBudgetUploaders().FirstOrDefault(o => o.ID == id);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }

        //    //ViewBag.LineManager = new SelectList((repoUsers.GetAll().Result != null) ? repoUsers.GetAll().Result.Result.Where(o => o.RoleId == (int)enuRole.LineManager) : null, "ID", "FullName", model.LineManagerID);

        //    return PartialView("_UpdateActivity", model);
        //}

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await repo.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }
            else if (entity != null)
            {
                await repo.Delete(entity); //Should not be deleted for now.
                //repo.Update(entity);
            }
            return RedirectToAction(nameof(Index));
        }


        //TODO: Source ==> https://www.talkingdotnet.com/import-export-excel-asp-net-core-2-razor-pages/

        //public ActionResult OnPostImport()
        //{
        //    IFormFile file = Request.Form.Files[0];
        //    string folderName = "Upload";
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string newPath = Path.Combine(webRootPath, folderName);
        //    StringBuilder sb = new StringBuilder();
        //    if (!Directory.Exists(newPath))
        //    {
        //        Directory.CreateDirectory(newPath);
        //    }
        //    if (file.Length > 0)
        //    {
        //        string sFileExtension = Path.GetExtension(file.FileName).ToLower();
        //        ISheet sheet;
        //        string fullPath = Path.Combine(newPath, file.FileName);
        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //            stream.Position = 0;
        //            if (sFileExtension == ".xls")
        //            {
        //                HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
        //                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
        //            }
        //            else
        //            {
        //                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
        //                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
        //            }
        //            IRow headerRow = sheet.GetRow(0); //Get Header Row
        //            int cellCount = headerRow.LastCellNum;
        //            sb.Append("<table class='table'><tr>");
        //            for (int j = 0; j < cellCount; j++)
        //            {
        //                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
        //                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
        //                sb.Append("<th>" + cell.ToString() + "</th>");
        //            }
        //            sb.Append("</tr>");
        //            sb.AppendLine("<tr>");
        //            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue;
        //                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
        //                for (int j = row.FirstCellNum; j < cellCount; j++)
        //                {
        //                    if (row.GetCell(j) != null)
        //                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
        //                }
        //                sb.AppendLine("</tr>");
        //            }
        //            sb.Append("</table>");
        //        }
        //    }
        //    return this.Content(sb.ToString());
        //}
    }
}
