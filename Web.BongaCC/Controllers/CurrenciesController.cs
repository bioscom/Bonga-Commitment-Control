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
    public class CurrenciesController : Controller
    {
        private readonly BongaCCDbContext _context;

        public CurrenciesController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: Currencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Currencies.ToListAsync());
        }

        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencies = await _context.Currencies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (currencies == null)
            {
                return NotFound();
            }

            return View(currencies);
        }

        // GET: Currencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Country,CurrencyName,Codes,Number,ID")] Currencies currencies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currencies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currencies);
        }

        // GET: Currencies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencies = await _context.Currencies.FindAsync(id);
            if (currencies == null)
            {
                return NotFound();
            }
            return View(currencies);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("Country,CurrencyName,Codes,Number,ID")] Currencies currencies)
        {
            if (id != currencies.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currencies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrenciesExists(currencies.ID))
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
            return View(currencies);
        }

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencies = await _context.Currencies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (currencies == null)
            {
                return NotFound();
            }

            return View(currencies);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var currencies = await _context.Currencies.FindAsync(id);
            _context.Currencies.Remove(currencies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrenciesExists(long? id)
        {
            return _context.Currencies.Any(e => e.ID == id);
        }
    }
}
