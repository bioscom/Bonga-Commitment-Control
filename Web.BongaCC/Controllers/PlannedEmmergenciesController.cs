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
    public class PlannedEmmergenciesController : Controller
    {
        private readonly BongaCCDbContext _context;

        public PlannedEmmergenciesController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: PlannedEmmergencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlannedEmmergency.ToListAsync());
        }

        // GET: PlannedEmmergencies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannedEmmergency = await _context.PlannedEmmergency
                .FirstOrDefaultAsync(m => m.ID == id);
            if (plannedEmmergency == null)
            {
                return NotFound();
            }

            return View(plannedEmmergency);
        }

        // GET: PlannedEmmergencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlannedEmmergencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanEmmerType,ID")] PlannedEmmergency plannedEmmergency)
        {
            if (ModelState.IsValid)
            {
                plannedEmmergency.AddedDate = DateTime.Now.Date;
                plannedEmmergency.ModifiedDate = DateTime.Now.Date;
                _context.Add(plannedEmmergency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plannedEmmergency);
        }

        // GET: PlannedEmmergencies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannedEmmergency = await _context.PlannedEmmergency.FindAsync(id);
            if (plannedEmmergency == null)
            {
                return NotFound();
            }
            return View(plannedEmmergency);
        }

        // POST: PlannedEmmergencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("PlanEmmerType,ID,AddedDate,ModifiedDate")] PlannedEmmergency plannedEmmergency)
        {
            if (id != plannedEmmergency.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plannedEmmergency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlannedEmmergencyExists(plannedEmmergency.ID))
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
            return View(plannedEmmergency);
        }

        // GET: PlannedEmmergencies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannedEmmergency = await _context.PlannedEmmergency
                .FirstOrDefaultAsync(m => m.ID == id);
            if (plannedEmmergency == null)
            {
                return NotFound();
            }

            return View(plannedEmmergency);
        }

        // POST: PlannedEmmergencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var plannedEmmergency = await _context.PlannedEmmergency.FindAsync(id);
            _context.PlannedEmmergency.Remove(plannedEmmergency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlannedEmmergencyExists(long? id)
        {
            return _context.PlannedEmmergency.Any(e => e.ID == id);
        }
    }
}
