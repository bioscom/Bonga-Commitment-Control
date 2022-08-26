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
using EF.BongaCC.Data.Migrations;

namespace Web.BongaCC.Controllers
{
    public class ActivityCodesController : Controller
    {
        private readonly IRepository<AppUsers> repoUsers;
        private readonly IRepository<ActivityCode> repo;
        private readonly IRepository<ActivityCodeWorkStream> repoWorkStream;

        public ActivityCodesController(IRepository<ActivityCode> repo, IRepository<ActivityCodeWorkStream> repoWorkStream, IRepository<AppUsers> repoUsers)
        {
            this.repo = repo;
            this.repoWorkStream = repoWorkStream;
            this.repoUsers = repoUsers;
        }

        public IEnumerable<ActivityCodeViewModel> GetActivityCodes()
        {
            var ActivityOwners = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).ToList();
            var LineManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).ToList();
            var AccountableManagers = repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).ToList();
            var Workstreams = repoWorkStream.GetAll().Result.ToList();

            try
            {
                var result = repo.GetAll().Result.OrderBy(o => o.ActivityCodeDesc).ToList().Select(entity =>
                {
                    var ActivityOwner = new AppUsers();
                    var LineManager = new AppUsers();
                    var AccountableManager = new AppUsers();
                    var Workstream = new ActivityCodeWorkStream();

                    if (ActivityOwners.FirstOrDefault(c => entity.ActivityOwnerID == c.ID) != null)
                    {
                        ActivityOwner = ActivityOwners.FirstOrDefault(c => entity.ActivityOwnerID == c.ID);
                    }

                    if (LineManagers.FirstOrDefault(c => entity.LineManagerID == c.ID) != null)
                    {
                        LineManager = LineManagers.FirstOrDefault(c => entity.LineManagerID == c.ID);
                    }

                    if (AccountableManagers.FirstOrDefault(c => entity.AccountableManagerID == c.ID) != null)
                    {
                        AccountableManager = AccountableManagers.FirstOrDefault(c => entity.AccountableManagerID == c.ID);
                    }

                    if (Workstreams.FirstOrDefault(c => entity.ActivityCodeWorkStreamID == c.ID) != null) Workstream = Workstreams.FirstOrDefault(c => entity.ActivityCodeWorkStreamID == c.ID);


                    //var ActivityOwner = (entity.ActivityOwnerID != null) ? ActivityOwners.FirstOrDefault(c => entity.ActivityOwnerID == c.ID) : null;
                    //var LineManager = (entity.LineManagerID != null) ? LineManagers.FirstOrDefault(c => entity.LineManagerID == c.ID) : null;
                    //var AccountableManager = (entity.AccountableManagerID != null) ? AccountableManagers.FirstOrDefault(c => entity.AccountableManagerID == c.ID) : null;

                    return new ActivityCodeViewModel
                    {
                        ID = entity.ID,
                        ActivityCodeDesc = entity.ActivityCodeDesc,
                        ActivityOwnerID = ActivityOwner.ID,
                        ActivityOwnerFullName = ActivityOwner.FullName,
                        LineManagerID = LineManager.ID,
                        LineManagerFullName = LineManager.FullName,
                        AccountableManagerID = AccountableManager.ID,
                        AccountableManagerFullName = AccountableManager.FullName,
                        ActivityCodeWorkStreamID = Workstream.ID,
                        WorkStreamName = Workstream.WorkStream,
                        WorkFlowType = Workstream.WorkFlowType,
                        WorkFlowTypeDesc = WorkFlowTypes.WorkFlowTypeDesc((WorkFlowTypes.enuWorkFlowType) Workstream.WorkFlowType),
                        //(entity.LineManagerID != null) ? repoUsers.Get(entity.LineManagerID).FullName : "N/A",
                        //LineManager = repoUsers.Get(entity.LineManagerID),
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
                var customerData = GetActivityCodes();  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.ActivityCodeDesc.ToUpper().Contains(searchValue) || (m.LineManagerFullName.ToUpper().Contains(searchValue))); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult LoadDataByActivityOwner(int? iUserId)
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
                var customerData = GetActivityCodes().Where(o => o.ActivityOwnerID == iUserId);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.ActivityCodeDesc.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult LoadDataByLineManager(int? iUserId)
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
                var customerData = GetActivityCodes().Where(o => o.LineManagerID == iUserId);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.ActivityCodeDesc.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult LoadDataByAccountableManager(int? iUserId)
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
                var customerData = GetActivityCodes().Where(o => o.AccountableManagerID == iUserId);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.ActivityCodeDesc.ToUpper().Contains(searchValue)); //Search  

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
            ActivityCodeViewModel model = new ActivityCodeViewModel();
            model.lstActivityCodes = GetActivityCodes();

            //ViewData["LineManagerID"]
            ViewBag.ActivityOwner = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(t => t.FullName), "ID", "FullName");
            ViewBag.LineManager = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).OrderBy(t => t.FullName), "ID", "FullName");
            ViewBag.AccountableManager = new SelectList(repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).OrderBy(t => t.FullName), "ID", "FullName");
            ViewBag.WorkStream = new SelectList(repoWorkStream.GetAll().Result.OrderBy(o => o.WorkStream), "ID", "WorkStream");

            return View(model);
        }

        // GET: ActivityCodes/Details/5
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityCode = GetActivityCodes().FirstOrDefault(m => m.ID == id);
            if (activityCode == null)
            {
                return NotFound();
            }

            return View(activityCode);
        }

        public async Task<IActionResult> AddEdit(ActivityCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNew = !model.ID.HasValue;
                ActivityCode entity = isNew ? new ActivityCode { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
                entity.ID = model.ID;
                entity.ActivityCodeDesc = model.ActivityCodeDesc;
                entity.LineManagerID = model.LineManagerID;
                entity.AccountableManagerID = model.AccountableManagerID;
                entity.ActivityOwnerID = model.ActivityOwnerID;
                entity.ActivityCodeWorkStreamID = model.ActivityCodeWorkStreamID;



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

        public async Task<IActionResult> AssignWorkstream(long Id, long WorkstreamId)
        {
            ActivityCode entity = await repo.GetById(Id);
            entity.ActivityCodeWorkStreamID = WorkstreamId;
            entity.ModifiedDate = DateTime.Today.Date;
            await repo.Update(entity);
            return RedirectToAction("Index");
        }


        // GET: ActivityCodes/Edit/5
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ActivityCodeViewModel model = GetActivityCodes().FirstOrDefault(o => o.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            ViewBag.ActivityOwner = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.ActivityOwner).OrderBy(t => t.FullName) : null, "ID", "FullName", model.ActivityOwnerID);
            ViewBag.LineManager = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.LineManager).OrderBy(t => t.FullName) : null, "ID", "FullName", model.LineManagerID);
            ViewBag.AccountableManager = new SelectList((repoUsers.GetAll() != null) ? repoUsers.GetAll().Result.Where(o => o.RoleId == (int)enuRole.AccountableManager).OrderBy(t => t.FullName) : null, "ID", "FullName", model.AccountableManagerID);
            ViewBag.WorkStream = new SelectList(repoWorkStream.GetAll().Result.OrderBy(o => o.WorkStream), "ID", "WorkStream", model.ActivityCodeWorkStreamID);

            return PartialView("_UpdateActivityCode", model);
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
