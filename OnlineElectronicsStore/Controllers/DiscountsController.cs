using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiscountsController : Controller
    {
        private readonly IDiscountService _discounts;

        public DiscountsController(IDiscountService discountService)
        {
            _discounts = discountService;
        }

        // GET /Discounts
        public async Task<IActionResult> Index()
        {
            var list = await _discounts.GetAllAsync();
            return View(list);
        }

        // GET /Discounts/Details/{code}
        public async Task<IActionResult> Details(string code)
        {
            var d = await _discounts.GetByCodeAsync(code);
            if (d == null) return NotFound();
            return View(d);
        }

        // GET /Discounts/Create
        public IActionResult Create() => View(new Discount());

        // POST /Discounts/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discount model)
        {
            if (!ModelState.IsValid) return View(model);
            await _discounts.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET /Discounts/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var all = await _discounts.GetAllAsync();
            var d = all.FirstOrDefault(x => x.Id == id);
            if (d == null) return NotFound();
            return View(d);
        }

        // POST /Discounts/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _discounts.DeleteAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
