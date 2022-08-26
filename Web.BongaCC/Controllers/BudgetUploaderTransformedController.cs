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
using System.Text;
using System.IO;
using System.Web;
using Web.BongaCC.Codes;
using Telerik.Windows.Documents.Flow.Model;

namespace Web.BongaCC.Controllers
{
    public class BudgetUploaderTransformedController : Controller
    {
        private readonly IRepository<BudgetUploaderTransformed> repo;
        private readonly IRepository<BudgetBook> repoBudgetBook;
        private readonly IRepository<BudgetBookFinanceYear> repoBudgetBookFinanceYear;

        private long? iRet = 0;

        public BudgetUploaderTransformedController(IRepository<BudgetUploaderTransformed> repo, IRepository<BudgetBook> repoBudgetBook, IRepository<BudgetBookFinanceYear> repoBudgetBookFinanceYear)
        {
            this.repo = repo;
            this.repoBudgetBook = repoBudgetBook;
            this.repoBudgetBookFinanceYear = repoBudgetBookFinanceYear;
        }

        public IActionResult Index()
        {
            BudgetUploaderViewModel model = new BudgetUploaderViewModel();
            return View(model);
        }

        private IEnumerable<BudgetUploaderViewModel> GetTransformedBudget()
        {
            int? iYear = DateTime.Today.Year;

            var result = repo.GetAll().Result.Where(o => o.YYear == iYear).OrderBy(o => o.ID).ToList().Select(entity =>
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
                var customerData = GetTransformedBudget(); // getting all Customer data
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

        //****************************************************************************************************************************
        //****************************************************************************************************************************
        //*** To uploade transformed data into BudgetBook, care should be made because, data alread existed in the Budgetbook.********
        //*** The best way to achieve this, is to compare all the parameters in each rows, where exists, update else insert   ********
        //****************************************************************************************************************************
        //****************************************************************************************************************************
        
        public async Task<IActionResult> UploadTranformedDataIntoBudgetBook()
        {
            IEnumerable<BudgetUploaderViewModel> result = GetTransformedBudget();

            foreach (BudgetUploaderViewModel model in result)
            {
                var item = repoBudgetBook.GetAll().Result.Where(t => t.CapexOpexID == long.Parse(model.ActivityType)
                                                            && t.DirectAllocated == int.Parse(model.DirectAllocated)
                                                            && t.UapCodeID == long.Parse(model.UapCode)
                                                            && t.UapRollUpCodeID == long.Parse(model.UapRollUpCode)
                                                            && t.ActivityTypeID == long.Parse(model.ActivityName)
                                                            && t.ActivityCodeID == long.Parse(model.ActivityCode)
                                                            && t.wbsID == long.Parse(model.CostCenter)
                                                            && t.ActivityID == long.Parse(model.Activity)
                                                            && t.ScopeID == long.Parse(model.ScopePurpose)
                                                            && t.ContractID == long.Parse(model.Contract)
                                                            && t.BudgetBasisID == long.Parse(model.Budgetbasis)
                                                            && t.YYear == DateTime.Today.Year);

                if (item.Count() > 0)
                {
                    var entity = item.FirstOrDefault();
                    await repoBudgetBook.GetById(entity.ID);
                    entity.NAPIMSBUDGETFDollar = model.OPYearBudget;
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repoBudgetBook.Update(entity);
                }
                else
                {
                    BudgetBook entity = new BudgetBook();
                    entity.ID = null;
                    entity.CapexOpexID = long.Parse(model.ActivityType);
                    entity.DirectAllocated = int.Parse(model.DirectAllocated);
                    entity.UapCodeID = long.Parse(model.UapCode);
                    entity.UapRollUpCodeID = long.Parse(model.UapRollUpCode);
                    entity.ActivityTypeID = long.Parse(model.ActivityName);
                    entity.ActivityCodeID = long.Parse(model.ActivityCode);
                    //entity.line = model.LineManager;
                    entity.wbsID = long.Parse(model.CostCenter);
                    entity.ActivityID = long.Parse(model.Activity);
                    //entity.ActivityOwnerID = long.Parse(model.ActivityOwner);
                    //entity.SponsorID = long.Parse(model.AccountableManager);
                    entity.ScopeID = long.Parse(model.ScopePurpose);
                    entity.ContractID = long.Parse(model.Contract);
                    entity.BudgetBasisID = long.Parse(model.Budgetbasis);
                    entity.NAPIMSBUDGETFDollar = model.OPYearBudget;

                    entity.YYear = DateTime.Today.Year;

                    entity.AddedDate = DateTime.Today.Date;
                    entity.ModifiedDate = DateTime.Today.Date;
                    iRet = await repoBudgetBook.Insert(entity);
                }
            }

            if (iRet > 0)
            {
                TempData["Message"] = "Budget Book Successfully updated.";
            }
            return RedirectToAction("Index", "BudgetBooks");
        }
    }
}