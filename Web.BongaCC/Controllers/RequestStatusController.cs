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
    public class RequestStatusController : Controller
    {
        private readonly BongaCCDbContext _context;

        public RequestStatusController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: RequestStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequestStatus.ToListAsync());
        }

        // GET: RequestStatus/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestStatus = await _context.RequestStatus
                .FirstOrDefaultAsync(m => m.ID == id);
            if (requestStatus == null)
            {
                return NotFound();
            }

            return View(requestStatus);
        }

        // GET: RequestStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequestStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReqstStatus,ID")] RequestStatus requestStatus)
        {
            if (ModelState.IsValid)
            {
                requestStatus.AddedDate = DateTime.Now.Date;
                requestStatus.ModifiedDate = DateTime.Now.Date;
                _context.Add(requestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requestStatus);
        }

        // GET: RequestStatus/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestStatus = await _context.RequestStatus.FindAsync(id);
            if (requestStatus == null)
            {
                return NotFound();
            }
            return View(requestStatus);
        }

        // POST: RequestStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("ReqstStatus,ID,AddedDate,ModifiedDate")] RequestStatus requestStatus)
        {
            if (id != requestStatus.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestStatusExists(requestStatus.ID))
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
            return View(requestStatus);
        }

        // GET: RequestStatus/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestStatus = await _context.RequestStatus
                .FirstOrDefaultAsync(m => m.ID == id);
            if (requestStatus == null)
            {
                return NotFound();
            }

            return View(requestStatus);
        }

        // POST: RequestStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var requestStatus = await _context.RequestStatus.FindAsync(id);
            _context.RequestStatus.Remove(requestStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestStatusExists(long? id)
        {
            return _context.RequestStatus.Any(e => e.ID == id);
        }
    }
}
