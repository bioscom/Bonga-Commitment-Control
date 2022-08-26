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
    public class FacilitiesController : Controller
    {
        private readonly BongaCCDbContext _context;

        public FacilitiesController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: Facilities
        public async Task<IActionResult> Index()
        {
            var bongaCCDbContext = _context.Facility.Include(f => f.Area);
            return View(await bongaCCDbContext.ToListAsync());
        }

        // GET: Facilities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility
                .Include(f => f.Area)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: Facilities/Create
        public IActionResult Create()
        {
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "AreaName");
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacilityName,Code,Agg,Location,AreaID,ID")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                facility.AddedDate = DateTime.Now.Date;
                facility.ModifiedDate = DateTime.Now.Date;
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "AreaName", facility.AreaID);
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "AreaName", facility.AreaID);
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("FacilityName,Code,Agg,Location,AreaID,ID,AddedDate,ModifiedDate")] Facility facility)
        {
            if (id != facility.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.ID))
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
            ViewData["AreaID"] = new SelectList(_context.Area, "ID", "AreaName", facility.AreaID);
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facility
                .Include(f => f.Area)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var facility = await _context.Facility.FindAsync(id);
            _context.Facility.Remove(facility);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(long? id)
        {
            return _context.Facility.Any(e => e.ID == id);
        }
    }
}
