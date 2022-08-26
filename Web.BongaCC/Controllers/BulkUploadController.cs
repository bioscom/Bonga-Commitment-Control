using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EF.BongaCC.Core.Model;
using EF.BongaCC.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Web.BongaCC.Controllers
{
    public class BulkUploadController : Controller
    {
        private readonly IRepository<BudgetUploader> repo;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        private readonly ISession _session;
        public ISession Session { get { return _session; } }

        public BulkUploadController(IRepository<BudgetUploader> repo, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this.repo = repo;
            _session = httpContextAccessor.HttpContext.Session;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upload(FormCollection formCollection)
        {
            var budgetBook = new List<BudgetUploader>();
            if (Request != null)
            {
                IFormFile file = Request.Form.Files["UploadedFile"];
                var memoryStream = new MemoryStream();

                if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    //Delete any data that belongs to the current year, before carrying out any update
                    var entities = repo.GetAll().Result.Where(t => t.YYear == DateTime.Today.Year);
                    foreach(BudgetUploader entity in entities)
                    {
                        await repo.Delete(entity);
                    }

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
                            //Note: the fist column on the Excelsheet should be S/No.
                            bbook.ActivityType = workSheet.Cells[rowIterator, 2].Value.ToString();          //Capex Opex
                            bbook.DirectAllocated = workSheet.Cells[rowIterator, 3].Value.ToString();       //Direct or Allocated
                            bbook.UapCode = workSheet.Cells[rowIterator, 4].Value.ToString();               //UapCode
                            bbook.UapRollUpCode = workSheet.Cells[rowIterator, 5].Value.ToString();         //UapRollUpCode
                            bbook.ActivityName = workSheet.Cells[rowIterator, 6].Value.ToString();          //ActivityName
                            bbook.ActivityCode = workSheet.Cells[rowIterator, 7].Value.ToString();          //ActivityCode
                            bbook.LineManager = workSheet.Cells[rowIterator, 8].Value.ToString();           //LineManager (Please, emailaddress)
                            bbook.CostCenter = workSheet.Cells[rowIterator, 9].Value.ToString();            //CostCenter
                            bbook.Activity = workSheet.Cells[rowIterator, 10].Value.ToString();             //Activity
                            bbook.ActivityOwner = workSheet.Cells[rowIterator, 11].Value.ToString();        //ActivityOwner
                            bbook.AccountableManager = workSheet.Cells[rowIterator, 12].Value.ToString();   //AccountableManager
                            bbook.ScopePurpose = workSheet.Cells[rowIterator, 13].Value.ToString();         //ScopePurpose
                            bbook.Contract = workSheet.Cells[rowIterator, 14].Value.ToString();             //Contract
                            bbook.Budgetbasis = workSheet.Cells[rowIterator, 15].Value.ToString();          //Budgetbasis
                            bbook.OPYearBudget = Convert.ToInt32(workSheet.Cells[rowIterator, 16].Value);   //NAPIMS FDollar
                            bbook.YYear = DateTime.Today.Year;                                              //Current year

                            budgetBook.Add(bbook);
                        }
                    }
                }
            }

            foreach (var item in budgetBook)
            {
                await repo.Insert(item);
            }

            return View("Index");
        }
    }
}