using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _products;

        public ProductsController(IProductService products)
        {
            _products = products;
        }

        // GET /Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await _products.GetAllAsync();
            if (model == null || !model.Any())
            {
                ViewBag.Message = "No products available.";
                return View("Empty");
            }
            return View(model);
        }

        // GET /Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null)
                return View("NotFound", id);
            return View(product);
        }

        // GET /Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Product());
        }

        // POST /Products/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _products.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET /Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST /Products/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(product);

            var updated = await _products.UpdateAsync(product);
            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET /Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _products.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
