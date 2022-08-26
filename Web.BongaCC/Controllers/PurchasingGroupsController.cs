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
    public class PurchasingGroupsController : Controller
    {
        private readonly BongaCCDbContext _context;

        public PurchasingGroupsController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: PurchasingGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchasingGroup.ToListAsync());
        }

        // GET: PurchasingGroups/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasingGroup = await _context.PurchasingGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchasingGroup == null)
            {
                return NotFound();
            }

            return View(purchasingGroup);
        }

        // GET: PurchasingGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchasingGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupName,ID")] PurchasingGroup purchasingGroup)
        {
            if (ModelState.IsValid)
            {
                purchasingGroup.AddedDate = DateTime.Now.Date;
                purchasingGroup.ModifiedDate = DateTime.Now.Date;
                _context.Add(purchasingGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchasingGroup);
        }

        // GET: PurchasingGroups/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasingGroup = await _context.PurchasingGroup.FindAsync(id);
            if (purchasingGroup == null)
            {
                return NotFound();
            }
            return View(purchasingGroup);
        }

        // POST: PurchasingGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("GroupName,ID,AddedDate,ModifiedDate")] PurchasingGroup purchasingGroup)
        {
            if (id != purchasingGroup.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchasingGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchasingGroupExists(purchasingGroup.ID))
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
            return View(purchasingGroup);
        }

        // GET: PurchasingGroups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasingGroup = await _context.PurchasingGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchasingGroup == null)
            {
                return NotFound();
            }

            return View(purchasingGroup);
        }

        // POST: PurchasingGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var purchasingGroup = await _context.PurchasingGroup.FindAsync(id);
            _context.PurchasingGroup.Remove(purchasingGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasingGroupExists(long? id)
        {
            return _context.PurchasingGroup.Any(e => e.ID == id);
        }
    }
}
