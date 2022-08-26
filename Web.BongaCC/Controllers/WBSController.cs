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
    public class WBSController : Controller
    {
        private readonly BongaCCDbContext _context;
        private readonly IRepository<WBS> repo;

        public WBSController(IRepository<WBS> repo, BongaCCDbContext context)
        {
            this.repo = repo;
            _context = context;
        }

        // GET: WBS
        public IActionResult Index()
        {
            WBSViewModel model = new WBSViewModel();
            model.lstWBS = GetCostObjects();

            return View(model);
        }

        private IEnumerable<WBSViewModel> GetCostObjects()
        {
            var result = repo.GetAll().Result.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                return new WBSViewModel
                {
                    ID = entity.ID,
                    CostObjects = entity.CostObjects,
                    CostObjectsDescription = entity.CostObjectsDescription,
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
                var customerData = GetCostObjects();  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.CostObjects.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> AddEdit(WBSViewModel model)
        {
            bool isNew = !model.ID.HasValue;
            WBS entity = isNew ? new WBS { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
            entity.ID = model.ID;
            entity.CostObjects = model.CostObjects;
            entity.CostObjectsDescription = model.CostObjectsDescription;

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

            WBSViewModel model = GetCostObjects().FirstOrDefault(o => o.ID == id);
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
    }
}
