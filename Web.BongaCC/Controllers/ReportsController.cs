using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Kendo.Mvc.UI;
using Web.BongaCC.Extensions;
using Kendo.Mvc.Extensions;

using EF.BongaCC.Core.Model;
using Web.BongaCC.ViewModels;
using EF.BongaCC.Data.Repository;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace Web.BongaCC.Controllers
{
    public class ReportsController : Controller
    {
        private IRepository<ExchangeRate> repoExchRate;
        private readonly CommitmentsController _commitmentsController;

        private ISession _session;
        public ISession Session { get { return _session; } }


        public ReportsController(CommitmentsController commitmentsController, IRepository<ExchangeRate> repoExchRate, IHttpContextAccessor httpContextAccessor)
        {
            this.repoExchRate = repoExchRate;
            _commitmentsController = commitmentsController;
            //_httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CommitmentControlViewModel model = new CommitmentControlViewModel();

            var result = repoExchRate.GetAll().Where(o => o.MMonth == DateTime.Today.Month && o.YYear == DateTime.Today.Year);
            model.ExchangeRate = (result.Count() == 0) ? repoExchRate.GetAll().LastOrDefault() : result.FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public IActionResult ExportReportToExcel(long? IdCapexOpex, long? IdCostObject, long? IdStatus, long? IdActivityOwner, long? IdActivityCode, long? IdActivityName, long? IdScope, long? IdBudgetBasis, DateTime? CCPSessionDateFrom, DateTime? CCPSessionDateTo)
        {
            var result = _commitmentsController.GetBudgetBookCommitments().Where(o => o.CapexOpexID == IdCapexOpex || o.wbsID == IdCostObject
                                                            || o.ApprovalID == IdStatus || o.ActivityOwnerID == IdActivityOwner
                                                            || o.ActivityCodeID == IdActivityCode || o.ActivityTypeID == IdActivityName
                                                            || o.ScopeID == IdScope || o.BudgetBasisID == IdBudgetBasis
                                                            || (o.CCPSessionDate == CCPSessionDateFrom && o.CCPSessionDate == CCPSessionDateTo));


            IEnumerable<BudgetBookCommitmentViewModel> Commit = result;

            byte[] content;
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //var contentType = "application/ms-excel";
            var fileName = "Commitments" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

            using (ExcelPackage excel = new ExcelPackage())
            {
                var oWorkSheet = excel.Workbook.Worksheets.Add("Bonga Commitment Control");
                oWorkSheet.TabColor = Color.Black;
                oWorkSheet.DefaultRowHeight = 12;

                int row = 1;

                oWorkSheet.Row(row).Height = 35;
                oWorkSheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                oWorkSheet.Row(row).Style.Font.Bold = true;
                oWorkSheet.Row(row).Style.Font.Size = 32;
                oWorkSheet.Cells[row, 3, row, 5].Value = "Bonga Commitment Control";
                oWorkSheet.Cells[row, 3, row, 5].Merge = true;
                oWorkSheet.Cells[row, 3].Style.WrapText = true;

                //row++;
                //oWs.Cells[row, 3, row, 5].Value = "";
                ////oWs.Cells[row, 3, row, 5].Value = (dte.HasValue) ? dte.Value.ToLongDateString() : DateTime.Today.Date.ToLongDateString();
                //oWs.Row(row).Style.Font.Size = 12;
                //oWs.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //oWs.Cells[row, 3, row, 5].Merge = true;
                //oWs.Cells[row, 3].Style.WrapText = true;

                row++;
                oWorkSheet.Cells[row, 1].Value = "S/No";
                oWorkSheet.Cells[row, 2].Value = "BCC No";
                oWorkSheet.Cells[row, 3].Value = "Activity Description";
                oWorkSheet.Cells[row, 4].Value = "CCP Session Date";
                oWorkSheet.Cells[row, 5].Value = "Commitment";
                oWorkSheet.Cells[row, 6].Value = "Capex/Opex";
                oWorkSheet.Cells[row, 7].Value = "Direct/Allocated";
                oWorkSheet.Cells[row, 8].Value = "UAP Code";
                oWorkSheet.Cells[row, 9].Value = "UAP Roll Up Code";
                oWorkSheet.Cells[row, 10].Value = "Activity Type";
                oWorkSheet.Cells[row, 11].Value = "Activity Code";
                oWorkSheet.Cells[row, 12].Value = "Cost Object";
                oWorkSheet.Cells[row, 13].Value = "Activity";
                oWorkSheet.Cells[row, 14].Value = "Focal Point";
                oWorkSheet.Cells[row, 15].Value = "Activity Owner";
                oWorkSheet.Cells[row, 16].Value = "Sponsor";
                oWorkSheet.Cells[row, 17].Value = "Cost Break Down";
                oWorkSheet.Cells[row, 18].Value = "Contract";
                oWorkSheet.Cells[row, 19].Value = "Scope";
                oWorkSheet.Cells[row, 20].Value = "Budget Basis";
                oWorkSheet.Cells[row, 21].Value = "OP Year Budget(F$)";
                oWorkSheet.Cells[row, 22].Value = "NAPIMS Budget(F$)";
                oWorkSheet.Cells[row, 23].Value = "FYLE (F$)";
                oWorkSheet.Cells[row, 24].Value = "Approval Decision";
                oWorkSheet.Cells[row, 25].Value = "Approval Decision Comments";
                oWorkSheet.Cells[row, 26].Value = "Savings($)";

                int i = 1;
                row++;
                foreach (BudgetBookCommitmentViewModel com in Commit)
                {
                    oWorkSheet.Cells[row, 1].Value = i++;
                    oWorkSheet.Cells[row, 2].Value = com.Comitmntno;
                    oWorkSheet.Cells[row, 3].Value = com.title;
                    oWorkSheet.Cells[row, 4].Value = com.CCPSessionDate;
                    oWorkSheet.Cells[row, 5].Value = com.Commitment;
                    oWorkSheet.Cells[row, 6].Value = com.CapexOpex;
                    oWorkSheet.Cells[row, 7].Value = com.sDirectAllocated;
                    oWorkSheet.Cells[row, 8].Value = com.UapCode;
                    oWorkSheet.Cells[row, 9].Value = com.UapRollUpCode;
                    oWorkSheet.Cells[row, 10].Value = com.ActivityType;
                    oWorkSheet.Cells[row, 11].Value = com.ActivityCode;
                    oWorkSheet.Cells[row, 12].Value = com.CostObject;
                    oWorkSheet.Cells[row, 13].Value = com.Activity;
                    oWorkSheet.Cells[row, 14].Value = com.FocalPoint;
                    oWorkSheet.Cells[row, 15].Value = com.ActivityOwner;
                    oWorkSheet.Cells[row, 16].Value = com.Sponsor;

                    #region   Cost Break Down Section

                    //IEnumerable<ActivityDetailsViewModel> Dtls = GetCostBreakdown().Where(o => o.CommitmentID == com.ID);
                    //if (Dtls.Count() > 0)
                    //{
                    //    oWs.Cells[row, 1].Value = "<Table border='1' bgColor='#ffffff' " +
                    //      "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                    //      "style='font-size:10.0pt; font-family:Calibri; background:white;'>";

                    //    oWs.Cells[row, 1].Value = "<tr>";
                    //    oWs.Cells[row, 1].Value = "<td>S/No</td>";
                    //    oWs.Cells[row, 1].Value = "<td>Description</td>";
                    //    oWs.Cells[row, 1].Value = "<td>Quantity</td>";
                    //    oWs.Cells[row, 1].Value = "<td>Rate ($)</td>";
                    //    oWs.Cells[row, 1].Value = "<td>Amount ($)</td>";
                    //    oWs.Cells[row, 1].Value = "</tr>";

                    //    int j = 1; decimal dAmount = 0; decimal dTotalAmount = 0;
                    //    foreach (ActivityDetailsViewModel oD in Dtls)
                    //    {
                    //        oWs.Cells[row, 1].Value = "<tr>";
                    //        oWs.Cells[row, 1].Value =j++;
                    //        oWs.Cells[row, 1].Value =oD.Description;
                    //        oWs.Cells[row, 1].Value =string.Format("{0:N}", oD.Quantity);
                    //        oWs.Cells[row, 1].Value =string.Format("{0:N}", oD.Rate);

                    //        dAmount = (oD.Quantity * oD.Rate);
                    //        dTotalAmount += dAmount;

                    //        oWs.Cells[row, 1].Value =string.Format("{0:N}", dAmount);
                    //        oWs.Cells[row, 1].Value = "</tr>";
                    //    }

                    //    oWs.Cells[row, 1].Value = "<tr>";
                    //    oWs.Cells[row, 1].Value = "<td></td>";
                    //    oWs.Cells[row, 1].Value = "<td>Total</td>";
                    //    oWs.Cells[row, 1].Value = "<td colspan='2'></td>";
                    //    oWs.Cells[row, 1].Value =string.Format("{0:N}", dTotalAmount);
                    //    oWs.Cells[row, 1].Value = "</tr>";
                    //    oWs.Cells[row, 1].Value = "</Table>";
                    //    row++;
                    //}
                    #endregion

                    oWorkSheet.Cells[row, 17].Value = "";
                    oWorkSheet.Cells[row, 18].Value = com.Contract;
                    oWorkSheet.Cells[row, 19].Value = com.Scope;
                    oWorkSheet.Cells[row, 20].Value = com.BudgetBasis;
                    oWorkSheet.Cells[row, 21].Value = com.OPYearBudgetFDollar;
                    oWorkSheet.Cells[row, 22].Value = com.NAPIMSBUDGETFDollar;
                    oWorkSheet.Cells[row, 23].Value = com.Q1FYLEFDollar;
                    oWorkSheet.Cells[row, 24].Value = com.ApprovalStatus;
                    oWorkSheet.Cells[row, 25].Value = com.ApprovalComment;
                    //oWorkSheet.Cells[row, 30].Value = string.Format("{0:N}", com.Commitment);
                    oWorkSheet.Cells[row, 26].Value = string.Format("{0:N}", com.Savings);
                    row++;
                }

                //oWs.PrinterSettings.PaperSize = ePaperSize.A3;
                //oWs.PrinterSettings.Orientation = eOrientation.Portrait;
                //oWs.PrinterSettings.FitToPage = true;
                //oWs.PrinterSettings.FitToHeight = 1;
                //oWs.PrinterSettings.TopMargin = .5M;
                //oWs.PrinterSettings.FooterMargin = .5M;
                //oWs.PrinterSettings.LeftMargin = .5M;
                //oWs.PrinterSettings.RightMargin = .5M;
                //oWs.Column(30).PageBreak = true;
                //oWs.PrinterSettings.Scale = 63;

                //var content = new MemoryStream(excel.GetAsByteArray());
                content = excel.GetAsByteArray();
            }

            if (content == null || content.Length == 0)
            {
                return NotFound();
            }
            //return File(content, contentType, $"TestFile.xlsx");
            //return File(fileContents: content, contentType: contentType, fileDownloadName: fileName);
            return File(content, contentType, fileName);
        }


        public ActionResult Excel_Export_Read([DataSourceRequest] DataSourceRequest request, long? IdCapexOpex, long? IdCostObject, long? IdStatus, long? IdActivityOwner, long? IdActivityCode, long? IdActivityName, long? IdScope, long? IdBudgetBasis, DateTime? CCPSessionDateFrom, DateTime? CCPSessionDateTo)
        {
            var model = Session.GetObjectFromJson<IEnumerable<BudgetBookCommitmentViewModel>>("Reporters");

            model = _commitmentsController.GetBudgetBookCommitments().Where(o => o.CapexOpexID == IdCapexOpex || o.wbsID == IdCostObject
                                                            || o.ApprovalID == IdStatus || o.ActivityOwnerID == IdActivityOwner
                                                            || o.ActivityCodeID == IdActivityCode || o.ActivityTypeID == IdActivityName
                                                            || o.ScopeID == IdScope || o.BudgetBasisID == IdBudgetBasis
                                                            || (o.CCPSessionDate == CCPSessionDateFrom && o.CCPSessionDate == CCPSessionDateTo));

            if (!model.Any())
            {
                model = _commitmentsController.GetBudgetBookCommitments();
            }

            Session.SetObjectAsJson("Reporters", model);
            return Json(model.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
                              //I will explain it later
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = "";// Path.Combine(Server.MapPath("~/temp"), file);

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                //filterContext.HttpContext.Response.Flush();

                //convert the current filter context to file and get the file path
                string filePath = ""; // (filterContext.Result as FilePathResult).FileName;

                //delete the file after download
                System.IO.File.Delete(filePath);
            }
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];

                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(data, contentType, fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}