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

namespace Web.BongaCC.Controllers
{
    public class ActivityTypesController : Controller
    {
        private readonly BongaCCDbContext _context;
        private readonly IRepository<ActivityType> repo;

        public ActivityTypesController(IRepository<ActivityType> repo, BongaCCDbContext context)
        {
            this.repo = repo;
            _context = context;
        }

        // GET: ActivityTypes
        public IActionResult Index()
        {
            ActivityTypeViewModel model = new ActivityTypeViewModel();
            model.lstActivityTypes = GetActivityTypes();

            return View(model);
        }

        private IEnumerable<ActivityTypeViewModel> GetActivityTypes()
        {
            var result = repo.GetAll().Result.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                return new ActivityTypeViewModel
                {
                    ID = entity.ID, 
                    ActivityName = entity.ActivityName,
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
                var customerData = GetActivityTypes();  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.ActivityName.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> AddEdit(ActivityTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNew = !model.ID.HasValue;
                ActivityType entity = isNew ? new ActivityType { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
                entity.ID = model.ID;
                entity.ActivityName = model.ActivityName;

                if (isNew)
                {
                    await repo.Insert(entity);
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

            ActivityTypeViewModel model = GetActivityTypes().FirstOrDefault(o => o.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            //ViewBag.LineManager = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Where(o => o.RoleId == (int)enuRole.LineManager) : null, "ID", "FullName", model.LineManagerID);

            return PartialView("_UpdateActivityType", model);
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
                //repo.Delete(entity); Should not be deleted for now.
                await repo.Update(entity);
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: ActivityTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: ActivityTypes/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ActivityName,ID,AddedDate,ModifiedDate")] ActivityType activityType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(activityType);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(activityType);
        //}

        //// GET: ActivityTypes/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var activityType = await _context.ActivityType.FindAsync(id);
        //    if (activityType == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(activityType);
        //}

        //// POST: ActivityTypes/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long? id, [Bind("ActivityName,ID,AddedDate,ModifiedDate")] ActivityType activityType)
        //{
        //    if (id != activityType.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(activityType);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ActivityTypeExists(activityType.ID))
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
        //    return View(activityType);
        //}

        //// GET: ActivityTypes/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var activityType = await _context.ActivityType
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (activityType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(activityType);
        //}

        //// POST: ActivityTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long? id)
        //{
        //    var activityType = await _context.ActivityType.FindAsync(id);
        //    _context.ActivityType.Remove(activityType);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ActivityTypeExists(long? id)
        //{
        //    return _context.ActivityType.Any(e => e.ID == id);
        //}
    }
}
