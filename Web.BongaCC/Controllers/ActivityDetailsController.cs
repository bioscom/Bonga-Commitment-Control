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
    public class ActivityDetailsController : Controller
    {
        private readonly BongaCCDbContext _context;

        public ActivityDetailsController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: ActivityDetails
        public async Task<IActionResult> Index()
        {
            var bongaCCDbContext = _context.ActivityDetails.Include(a => a.BudgetBookCommitments);
            return View(await bongaCCDbContext.ToListAsync());
        }

        // GET: ActivityDetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityDetails = await _context.ActivityDetails
                .Include(a => a.BudgetBookCommitments)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityDetails == null)
            {
                return NotFound();
            }

            return View(activityDetails);
        }

        // GET: ActivityDetails/Create
        public IActionResult Create()
        {
            //ViewData["CommitmentID"] = new SelectList(_context.Commitments, "ID", "ID");
            return View();
        }

        // POST: ActivityDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Quantity,Rate,CommitmentID,ID")] ActivityDetails activityDetails)
        {
            if (ModelState.IsValid)
            {
                activityDetails.AddedDate = DateTime.Now.Date;
                activityDetails.ModifiedDate = DateTime.Now.Date;
                _context.Add(activityDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CommitmentID"] = new SelectList(_context.Commitments, "ID", "ID", activityDetails.CommitmentID);
            return View(activityDetails);
        }

        // GET: ActivityDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityDetails = await _context.ActivityDetails.FindAsync(id);
            if (activityDetails == null)
            {
                return NotFound();
            }
            //ViewData["CommitmentID"] = new SelectList(_context.Commitments, "ID", "ID", activityDetails.CommitmentID);
            return View(activityDetails);
        }

        // POST: ActivityDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("Description,Quantity,Rate,CommitmentID,ID,AddedDate,ModifiedDate")] ActivityDetails activityDetails)
        {
            if (id != activityDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityDetailsExists(activityDetails.ID))
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
            //ViewData["CommitmentID"] = new SelectList(_context.Commitments, "ID", "ID", activityDetails.CommitmentID);
            return View(activityDetails);
        }

        // GET: ActivityDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityDetails = await _context.ActivityDetails
                .Include(a => a.BudgetBookCommitments)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activityDetails == null)
            {
                return NotFound();
            }

            return View(activityDetails);
        }

        // POST: ActivityDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var activityDetails = await _context.ActivityDetails.FindAsync(id);
            _context.ActivityDetails.Remove(activityDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //Delete
        }

        private bool ActivityDetailsExists(long? id)
        {
            return _context.ActivityDetails.Any(e => e.ID == id);
        }
    }
}
