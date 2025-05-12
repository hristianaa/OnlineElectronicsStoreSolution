using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly AppDbContext _context;
        public AdminProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /AdminProducts
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .ToListAsync();
            return View(products);
        }

        // GET: /AdminProducts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: /AdminProducts/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDownList();
            return View(new Product());
        }

        // POST: /AdminProducts/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropDownList(product.CategoryId);
                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        // POST: /AdminProducts/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropDownList(product.CategoryId);
                return View(product);
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: /AdminProducts/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helpers

        private bool ProductExists(int id) =>
            _context.Products.Any(p => p.Id == id);

        private async Task PopulateCategoriesDropDownList(object? selectedCategory = null)
        {
            var categories = await _context.Categories
                                           .OrderBy(c => c.Name)
                                           .ToListAsync();
            ViewBag.CategoryId = new SelectList(categories, nameof(Category.Id), nameof(Category.Name), selectedCategory);
        }
    }
}
