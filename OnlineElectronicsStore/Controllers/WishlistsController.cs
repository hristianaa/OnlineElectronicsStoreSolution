using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]  // only signed‐in users can manage wishlists
    public class WishlistsController : Controller
    {
        private readonly AppDbContext _context;
        public WishlistsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Wishlists
        public async Task<IActionResult> Index()
        {
            var list = await _context.Wishlists
                                     .OrderBy(w => w.Id)
                                     .ToListAsync();
            return View(list);
        }

        // GET: /Wishlists/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Wishlists
                                     .FirstOrDefaultAsync(w => w.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Wishlists/Create
        public IActionResult Create()
        {
            return View(new Wishlist());
        }

        // POST: /Wishlists/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Wishlist model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Wishlists.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Wishlists/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Wishlists.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Wishlists/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Wishlist model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Wishlists.AnyAsync(w => w.Id == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Wishlists/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Wishlists
                                     .FirstOrDefaultAsync(w => w.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Wishlists/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Wishlists.FindAsync(id);
            if (item != null)
            {
                _context.Wishlists.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
