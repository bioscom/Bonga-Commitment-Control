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
using Web.BongaCC.Codes;

namespace Web.BongaCC.Controllers
{
    public class ActivityCodesWorkStreamController : Controller
    {
        private readonly IRepository<ActivityCodeWorkStream> repo;

        public ActivityCodesWorkStreamController(IRepository<ActivityCodeWorkStream> repo)
        {
            this.repo = repo;
        }

        public IEnumerable<ActivityCodeWorkStreamViewModel> GetActivityCodeWorkStreams()
        {
            try
            {
                var result = repo.GetAll().Result.OrderBy(o => o.WorkStream).ToList().Select(entity =>
                {
                    return new ActivityCodeWorkStreamViewModel
                    {
                        ID = entity.ID,
                        WorkStream = entity.WorkStream,
                        WorkStreamDesc = entity.WorkStreamDesc,
                        WorkFlowType = entity.WorkFlowType,
                        sWorkFlowType = WorkFlowTypes.WorkFlowTypeDesc((WorkFlowTypes.enuWorkFlowType)entity.WorkFlowType),
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                var customerData = GetActivityCodeWorkStreams();  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.WorkStream.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: ActivityCodes
        public IActionResult Index()
        {
            ActivityCodeWorkStreamViewModel model = new ActivityCodeWorkStreamViewModel();
            ViewBag.WorkFlowType = new SelectList(RolesManager.GetAllWorkFlowTypes().OrderBy(o => o.Text), "Value", "Text");

            model.lstActivityCodesWS = GetActivityCodeWorkStreams();
            return View(model);
        }

        // GET: ActivityCodes/Details/5
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workstream = GetActivityCodeWorkStreams().FirstOrDefault(m => m.ID == id);
            if (workstream == null)
            {
                return NotFound();
            }

            return View(workstream);
        }

        public async Task<IActionResult> AddEdit(ActivityCodeWorkStreamViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNew = !model.ID.HasValue;
                ActivityCodeWorkStream entity = isNew ? new ActivityCodeWorkStream { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
                entity.ID = model.ID;
                entity.WorkStream = model.WorkStream;
                entity.WorkStreamDesc = model.WorkStreamDesc;
                entity.WorkFlowType = model.WorkFlowType;

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

            ActivityCodeWorkStreamViewModel model = GetActivityCodeWorkStreams().FirstOrDefault(o => o.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            ViewBag.WorkFlowType = new SelectList(RolesManager.GetAllWorkFlowTypes().OrderBy(o => o.Text), "Value", "Text");

            return PartialView("_UpdateActivityCodeWS", model);
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
