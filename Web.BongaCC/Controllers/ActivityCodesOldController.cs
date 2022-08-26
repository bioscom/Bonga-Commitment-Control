using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF.BongaCC.Core.Model;
using EF.BongaCC.Data;

namespace Web.BongaCC.Controllers
{
    public class ActivityCodesController : Controller
    {
        private readonly BongaCCDbContext _context;

        public ActivityCodesController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: ActivityCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityCode.ToListAsync());
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
                var searchValue = Request.Form["search[value]"].FirstOrDefault(); // Search Value from (Search box)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0; //Paging Size (10, 20, 50,100)  
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = _context.ActivityCode;  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                //if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Description == searchValue); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: ActivityCodes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityCode = await _context.ActivityCode
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityCode == null)
            {
                return NotFound();
            }

            return View(activityCode);
        }

        // GET: ActivityCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityCodeDesc,ID,AddedDate,ModifiedDate")] ActivityCode activityCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityCode);
        }

        // GET: ActivityCodes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityCode = await _context.ActivityCode.FindAsync(id);
            if (activityCode == null)
            {
                return NotFound();
            }
            return View(activityCode);
        }

        // POST: ActivityCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("ActivityCodeDesc,ID,AddedDate,ModifiedDate")] ActivityCode activityCode)
        {
            if (id != activityCode.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityCodeExists(activityCode.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activityCode);
        }

        // GET: ActivityCodes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityCode = await _context.ActivityCode
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityCode == null)
            {
                return NotFound();
            }

            return View(activityCode);
        }

        // POST: ActivityCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var activityCode = await _context.ActivityCode.FindAsync(id);
            _context.ActivityCode.Remove(activityCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityCodeExists(long? id)
        {
            return _context.ActivityCode.Any(e => e.ID == id);
        }
    }
}
