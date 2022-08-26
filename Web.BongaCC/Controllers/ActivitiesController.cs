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

namespace Web.BongaCC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly BongaCCDbContext _context;
        private readonly IRepository<Activity> repo;

        public ActivitiesController(IRepository<Activity> repo, BongaCCDbContext context)
        {
            this.repo = repo;
            _context = context;
        }

        // GET: Activities
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Activity.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            ActivityViewModel model = new ActivityViewModel
            {
                lstActivities = await GetActivities()
            };

            return View(model);
        }

        private async Task<IEnumerable<ActivityViewModel>> GetActivities()
        {
            //int? iYyear = (iYear == null) ? iYear = DateTime.Today.Year : iYear;

            var result = repo.GetAll().Result.OrderBy(o => o.Description).ToList().Select(entity =>
            {
                return new ActivityViewModel
                {
                    ID = entity.ID,
                    Description = entity.Description,
                };
            }).ToList();
            return result;
        }

        public async Task<IActionResult> LoadData()
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
                var customerData = await GetActivities(); // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Description.ToUpper().Contains(searchValue)); //Search  
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


        public async Task<IActionResult> AddEdit(ActivityViewModel model)
        {
            bool isNew = !model.ID.HasValue;
            Activity entity = isNew ? new Activity { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
            entity.ID = model.ID;
            entity.Description = model.Description;

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

        // GET: ActivityCodes/Edit/5
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ActivityViewModel model = GetActivities().Result.FirstOrDefault(o => o.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            //ViewBag.LineManager = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Where(o => o.RoleId == (int)enuRole.LineManager) : null, "ID", "FullName", model.LineManagerID);

            return PartialView("_UpdateActivity", model);
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


        // GET: Activities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FirstOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }
    }
}
