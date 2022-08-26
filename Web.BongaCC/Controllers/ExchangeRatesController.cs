using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF.BongaCC.Core.Model;
using EF.BongaCC.Data;
using EF.BongaCC.Data.Repository;
using Web.BongaCC.ViewModels;
using System.Globalization;
using Web.BongaCC.Codes;
using Microsoft.AspNetCore.Http;

namespace Web.BongaCC.Controllers
{
    public class ExchangeRatesController : Controller
    {
        private readonly IRepository<ExchangeRate> repo;

        public ExchangeRatesController(IRepository<ExchangeRate> repo)
        {
            this.repo = repo;
        }

        //// GET: ExchangeRates
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.ExchangeRate.ToListAsync());
        //}

        public IActionResult Index()
        {
            ExchangeRatesViewModel model = new ExchangeRatesViewModel();
            model.lstExchangeRates = GetExchangeRates();
            ViewBag.Months = new SelectList(Computations.GetMonths().ToList(), "Month", "MonthName");

            return View(model);
        }

        private IEnumerable<ExchangeRatesViewModel> GetExchangeRates()
        {
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            int iYyear = DateTime.Today.Year;
            //var result = repo.GetAll().Where(m => m.YYear == iYyear).OrderByDescending(o => o.iDay).ThenByDescending(o => o.MMonth).ToList().Select(entity =>
            var result = repo.GetAll().Result.Where(m => m.YYear == iYyear).OrderBy(o => o.MMonth).ToList().Select(entity =>
            {
                return new ExchangeRatesViewModel
                    {
                        ID = entity.ID,
                        FloatingExchangeRate = entity.FloatingExchangeRate,
                        iDay = entity.iDay,
                        sMonth = mfi.GetMonthName(entity.MMonth).ToString(),
                        MMonth = entity.MMonth,
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
                var customerData = GetExchangeRates(); // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.sMonth.Contains(searchValue)); //Search  
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


        public async Task<IActionResult> AddEdit(ExchangeRatesViewModel model)
        {
            int iMonth = model.MMonth; //int.Parse(formCollection["ddlMonths"]); //Convert.ToInt32(Collection["ddlMonths"]); //ViewBag.Months;
            bool isNew = !model.ID.HasValue;
            ExchangeRate entity = isNew ? new ExchangeRate { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
            entity.ID = model.ID;
            entity.FloatingExchangeRate = model.FloatingExchangeRate;
            entity.iDay = DateTime.Today.Day;
            entity.MMonth = iMonth;
            //entity.MMonth = DateTime.Today.Month;
            entity.YYear = DateTime.Today.Year;

            if (isNew)
            {
                IEnumerable<ExchangeRate> r = repo.GetAll().Result.Where(m => m.iDay == entity.iDay && (m.MMonth == entity.MMonth) && (m.YYear == entity.YYear));
                if (r.Count() > 0)
                {
                    TempData["Message"] = "Entry already exists for today.";
                }
                else
                {
                    await repo.Insert(entity);
                }
            }
            else
            {
                if (model.iDay < DateTime.Today.Day)
                {
                    TempData["Message"] = "Operation not allowed.";
                }
                else
                {
                    entity.ModifiedDate = DateTime.Today.Date;
                    await repo.Update(entity);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: ActivityCodes/Edit/5
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExchangeRatesViewModel model = GetExchangeRates().FirstOrDefault(o => o.ID == id);
            ViewBag.Months = new SelectList(Computations.GetMonths().ToList(), "Month", "MonthName", model.MMonth);
            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_UpdateExchangeRate", model);
        }

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

        //// GET: ExchangeRates/Details/5
        //public async Task<IActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var exchangeRate = await _context.ExchangeRate
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (exchangeRate == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(exchangeRate);
        //}

        //// GET: ExchangeRates/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ExchangeRates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ExchangeRateValue,YYear,MMonth,ID")] ExchangeRate exchangeRate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        exchangeRate.AddedDate = DateTime.Now.Date;
        //        exchangeRate.ModifiedDate = DateTime.Now.Date;

        //        _context.Add(exchangeRate);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(exchangeRate);
        //}

        //// GET: ExchangeRates/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var exchangeRate = await _context.ExchangeRate.FindAsync(id);
        //    if (exchangeRate == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(exchangeRate);
        //}

        //// POST: ExchangeRates/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long? id, [Bind("ExchangeRateValue,YYear,MMonth,ID")] ExchangeRate exchangeRate)
        //{
        //    if (id != exchangeRate.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            exchangeRate.ModifiedDate = DateTime.Now.Date;
        //            _context.Update(exchangeRate);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ExchangeRateExists(exchangeRate.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(exchangeRate);
        //}

        //// GET: ExchangeRates/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var exchangeRate = await _context.ExchangeRate
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (exchangeRate == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(exchangeRate);
        //}

        //// POST: ExchangeRates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long? id)
        //{
        //    var exchangeRate = await _context.ExchangeRate.FindAsync(id);
        //    _context.ExchangeRate.Remove(exchangeRate);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ExchangeRateExists(long? id)
        //{
        //    return _context.ExchangeRate.Any(e => e.ID == id);
        //}
    }
}
