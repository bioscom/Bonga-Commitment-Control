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
    public class ContractProcurementVehiclesController : Controller
    {
        private readonly BongaCCDbContext _context;

        public ContractProcurementVehiclesController(BongaCCDbContext context)
        {
            _context = context;
        }

        // GET: ContractProcurementVehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContractProcurementVehicle.ToListAsync());
        }

        // GET: ContractProcurementVehicles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractProcurementVehicle = await _context.ContractProcurementVehicle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contractProcurementVehicle == null)
            {
                return NotFound();
            }

            return View(contractProcurementVehicle);
        }

        // GET: ContractProcurementVehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContractProcurementVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleName,ID")] ContractProcurementVehicle contractProcurementVehicle)
        {
            if (ModelState.IsValid)
            {
                contractProcurementVehicle.AddedDate = DateTime.Now.Date;
                contractProcurementVehicle.ModifiedDate = DateTime.Now.Date;
                _context.Add(contractProcurementVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractProcurementVehicle);
        }

        // GET: ContractProcurementVehicles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractProcurementVehicle = await _context.ContractProcurementVehicle.FindAsync(id);
            if (contractProcurementVehicle == null)
            {
                return NotFound();
            }
            return View(contractProcurementVehicle);
        }

        // POST: ContractProcurementVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("VehicleName,ID,AddedDate,ModifiedDate")] ContractProcurementVehicle contractProcurementVehicle)
        {
            if (id != contractProcurementVehicle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractProcurementVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractProcurementVehicleExists(contractProcurementVehicle.ID))
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
            return View(contractProcurementVehicle);
        }

        // GET: ContractProcurementVehicles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractProcurementVehicle = await _context.ContractProcurementVehicle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contractProcurementVehicle == null)
            {
                return NotFound();
            }

            return View(contractProcurementVehicle);
        }

        // POST: ContractProcurementVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var contractProcurementVehicle = await _context.ContractProcurementVehicle.FindAsync(id);
            _context.ContractProcurementVehicle.Remove(contractProcurementVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractProcurementVehicleExists(long? id)
        {
            return _context.ContractProcurementVehicle.Any(e => e.ID == id);
        }
    }
}
