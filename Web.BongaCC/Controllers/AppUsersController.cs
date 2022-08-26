using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EF.BongaCC.Core.Model;
using EF.BongaCC.Data.Repository;
using Web.BongaCC.ViewModels;
using Web.BongaCC.Codes;
using System.Threading.Tasks;

namespace Web.BongaCC.Controllers
{
    public partial class AppUsersController : Controller
    {
        private IRepository<AppUsers> repo;
        private IRepository<ActivityCode> repoActivityCode;

        public AppUsersController(IRepository<AppUsers> repo, IRepository<ActivityCode> repoActivityCode)
        {
            this.repo = repo;
            this.repoActivityCode = repoActivityCode;
        }

        public IEnumerable<UserManagementViewModel> GetUsers()
        {
            var result = repo.GetAll().Result.OrderBy(o => o.FullName).ToList().Select(entity =>
            {
                return new UserManagementViewModel
                {
                    ID = entity.ID,
                    Deligate = entity.Deligate,
                    FullName = entity.FullName,
                    IsGuestAcct = entity.IsGuestAcct,
                    LoginTime = entity.LoginTime,
                    Password = entity.Password,
                    RefInd = entity.RefInd,
                    RoleId = entity.RoleId,
                    Status = entity.Status,
                    UserMail = entity.UserMail,
                    UserName = entity.UserName,
                    Roles = ((enuRole)(Convert.ToInt16(entity.RoleId))).ToString(),
                    sStatus = ((enuStatus)(Convert.ToInt16(entity.Status))).ToString(),
                    //LineManagerID = entity.LineManagerID,
                    //LineManagerFullName = (entity.LineManagerID != null) ? repo.Get(entity.LineManagerID).FullName : "N/A",
                };
            }).ToList();
            return result;
        }

        public IActionResult LoadData(int? Id)
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
                var customerData = (Id == null) ? GetUsers().Where(o => o.Status == (int)enuStatus.Active) : GetUsers().Where(o => o.RoleId == Id && o.Status == (int)enuStatus.Active);  // getting all active users
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.FullName.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: AppUsers
        public IActionResult Index()
        {
            UserManagementViewModel model = new UserManagementViewModel();

            model.lstUsers = GetUsers();

            ViewBag.Status = new SelectList(RolesManager.GetAllStatus(), "Value", "Text");
            ViewBag.Roles = new SelectList(RolesManager.GetAllRoles().OrderBy(o => o.Text), "Value", "Text");
            //ViewBag.LineMgrs = new SelectList(GetUsers().Where(o => o.RoleId == (int)enuRole.LineManager), "ID", "FullName");

            return View(model);
        }

        // GET: AppUsers/Details/5
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUsers = GetUsers().FirstOrDefault(m => m.ID == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            return View(appUsers);
        }

        public async Task<IActionResult> AddEdit(UserManagementViewModel model)
        {
            bool isNew = !model.ID.HasValue;
            AppUsers entity = isNew ? new AppUsers { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);

            entity.ID = model.ID;
            entity.FullName = model.FullName;
            entity.RefInd = model.RefInd;
            entity.UserMail = model.UserMail;
            entity.UserName = model.UserName;

            if (isNew)
            {
                IEnumerable<AppUsers> found = repo.GetAll().Result.Where(o => o.UserMail.ToLower().Trim() == model.UserMail.ToLower().Trim());
                if (found.Count() > 0)
                {
                    var me = found.FirstOrDefault();
                    TempData["Message"] = me.FullName + " already exists in the database as " + ((enuRole)(Convert.ToInt16(me.RoleId))).ToString() + ", double entry not allowed.";
                    return RedirectToAction("Index");
                }

                entity.RoleId = model.RoleId;
                entity.Status = (int)enuStatus.Active;
                entity.ModifiedDate = DateTime.Today.Date;

                entity.Deligate = 1;
                entity.IsGuestAcct = true;
                entity.LoginTime = DateTime.Today.Date;

                await repo.Insert(entity);
            }
            else
            {
                entity.RoleId = model.RoleId;
                entity.Status = model.Status;
                entity.ModifiedDate = DateTime.Today.Date;
                await repo.Update(entity);
            }
            return RedirectToAction("Index");
        }

        // GET: AppUsers/Edit/5
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUsers = GetUsers().FirstOrDefault(o => o.ID == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            ViewBag.Status = new SelectList(RolesManager.GetAllStatus(), "Value", "Text", appUsers.Status);
            ViewBag.Roles = new SelectList(RolesManager.GetAllRoles(), "Value", "Text", appUsers.RoleId);

            return PartialView("_UpdateUser", appUsers);
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
                entity.Status = (int)enuStatus.Deactivated;
                await repo.Update(entity);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
