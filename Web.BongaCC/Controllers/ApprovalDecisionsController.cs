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
    public class ApprovalDecisionsController : Controller
    {
        private IRepository<ApprovalDecision> repo;

        public ApprovalDecisionsController(IRepository<ApprovalDecision> repo)
        {
            this.repo = repo;
        }

        public IEnumerable<ApprovalDecisionsViewModel> GetApprovalDecisions()
        {
            var result = repo.GetAll().Result.OrderBy(o => o.ID).ToList().Select(entity =>
            {
                return new ApprovalDecisionsViewModel
                {
                    ID = entity.ID,
                    Decision = entity.Decision,
                    ColorCode = entity.ColorCode,
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
                var customerData = GetApprovalDecisions();  // getting all active users
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection;
                }
                if (!string.IsNullOrEmpty(searchValue)) customerData = customerData.Where(m => m.Decision.ToUpper().Contains(searchValue)); //Search  

                recordsTotal = customerData.Count(); //total number of rows counts   
                var data = customerData.Skip(skip).Take(pageSize).ToList();  //Paging   
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }); //Returning Json Data  
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: ApprovalDecisions
        public IActionResult Index()
        {
            ApprovalDecisionsViewModel model = new ApprovalDecisionsViewModel();

            model.lstApprovalDecisions = GetApprovalDecisions(); 

            return View(model);
        }

        public async Task<IActionResult> AddEdit(ApprovalDecisionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNew = !model.ID.HasValue;
                ApprovalDecision entity = isNew ? new ApprovalDecision { AddedDate = DateTime.Today.Date } : await repo.GetById(model.ID);

                entity.ID = model.ID;
                entity.Decision = model.Decision;
                entity.ColorCode = model.ColorCode;

                if (isNew)
                {
                    IEnumerable<ApprovalDecision> found = repo.GetAll().Result.Where(o => o.Decision.ToLower().Trim() == model.Decision.ToLower().Trim());
                    if (found.Count() > 0)
                    {
                        var me = found.FirstOrDefault();
                        TempData["Message"] = me.Decision + " already exists in the database, double entry not allowed.";
                        return RedirectToAction("Index");
                    }
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

        // GET: AppUsers/Edit/5
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalDecision = GetApprovalDecisions().FirstOrDefault(o => o.ID == id);
            if (approvalDecision == null)
            {
                return NotFound();
            }

            return PartialView("_UpdateApprovalDecision", approvalDecision);
        }

        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var approvalDecision = await _context.Discipline.FirstOrDefaultAsync(m => m.ID == id);
        //    //if (approvalDecision == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return View(approvalDecision);
        //}

        //// POST: ApprovalDecisions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Decision,ColorCode,ID")] ApprovalDecision approvalDecision)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        approvalDecision.AddedDate = DateTime.Now.Date;
        //        approvalDecision.ModifiedDate = DateTime.Now.Date;
        //        _context.Add(approvalDecision);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(approvalDecision);
        //}

        //// GET: ApprovalDecisions/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var approvalDecision = await _context.Discipline.FindAsync(id);
        //    if (approvalDecision == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(approvalDecision);
        //}

        //// POST: ApprovalDecisions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long? id, [Bind("Decision,ColorCode,ID,AddedDate,ModifiedDate")] ApprovalDecision approvalDecision)
        //{
        //    if (id != approvalDecision.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(approvalDecision);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApprovalDecisionExists(approvalDecision.ID))
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
        //    return View(approvalDecision);
        //}

        // GET: ApprovalDecisions/Delete/5


        //// POST: ApprovalDecisions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long? id)
        //{
        //    var approvalDecision = await _context.Discipline.FindAsync(id);
        //    _context.Discipline.Remove(approvalDecision);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ApprovalDecisionExists(long? id)
        //{
        //    return _context.Discipline.Any(e => e.ID == id);
        //}
    }
}
