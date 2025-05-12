using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShippingDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /ShippingDetails
        public async Task<IActionResult> Index()
        {
            var list = await _context.ShippingDetails
                                     .OrderBy(s => s.Id)
                                     .ToListAsync();
            return View(list);
        }

        // GET: /ShippingDetails/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var detail = await _context.ShippingDetails
                                       .FirstOrDefaultAsync(s => s.Id == id);
            if (detail == null) return NotFound();
            return View(detail);
        }

        // GET: /ShippingDetails/Create
        public IActionResult Create()
        {
            return View(new ShippingDetails());
        }

        // POST: /ShippingDetails/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingDetails model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.ShippingDetails.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /ShippingDetails/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var detail = await _context.ShippingDetails.FindAsync(id);
            if (detail == null) return NotFound();
            return View(detail);
        }

        // POST: /ShippingDetails/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShippingDetails model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ShippingDetails.AnyAsync(s => s.Id == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ShippingDetails/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var detail = await _context.ShippingDetails
                                       .FirstOrDefaultAsync(s => s.Id == id);
            if (detail == null) return NotFound();
            return View(detail);
        }

        // POST: /ShippingDetails/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detail = await _context.ShippingDetails.FindAsync(id);
            if (detail != null)
            {
                _context.ShippingDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
