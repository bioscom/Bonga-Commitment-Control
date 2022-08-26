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
    public class BudgetBasisController : Controller
    {
        private readonly BongaCCDbContext _context;
        private readonly IRepository<BudgetBasis> repo;

        public BudgetBasisController(IRepository<BudgetBasis> repo, BongaCCDbContext context)
        {
            this.repo = repo;
            _context = context;
        }

        // GET: BudgetBasis
        public IActionResult Index()
        {
            BudgetBaseViewModel model = new BudgetBaseViewModel();
            model.lstBudgetBases = GetBudgetBase();

            return View(model);
        }

        private IEnumerable<BudgetBaseViewModel> GetBudgetBase()
        {
            var result = repo.GetAll().Result.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                return new BudgetBaseViewModel
                {
                    ID = entity.ID,
                    BudgetBase = entity.BudgetBase,
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
                var customerData = GetBudgetBase();  // getting all Customer data
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.BudgetBase.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IActionResult> AddEdit(BudgetBaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNew = !model.ID.HasValue;
                BudgetBasis entity = isNew ? new BudgetBasis { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);
                entity.ID = model.ID;
                entity.BudgetBase = model.BudgetBase;

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

            BudgetBaseViewModel model = GetBudgetBase().FirstOrDefault(o => o.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_UpdateBudgetBasis", model);
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

        // GET: BudgetBasis/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetBasis = await _context.BudgetBasis
                .FirstOrDefaultAsync(m => m.ID == id);
            if (budgetBasis == null)
            {
                return NotFound();
            }

            return View(budgetBasis);
        }

        // GET: BudgetBasis/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
