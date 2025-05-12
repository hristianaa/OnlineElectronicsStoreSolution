using System.Linq;
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
        private readonly IDiscountService _discountService;
        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: /Discounts
        public IActionResult Index()
        {
            var discounts = _discountService.GetAll().ToList();
            return View(discounts);
        }

        // GET: /Discounts/Details/ABC123
        public IActionResult Details(string code)
        {
            var discount = _discountService.GetByCode(code);
            if (discount == null) return NotFound();
            return View(discount);
        }

        // GET: /Discounts/Create
        public IActionResult Create()
        {
            return View(new Discount());
        }

        // POST: /Discounts/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Discount model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _discountService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Discounts/Delete/5
        public IActionResult Delete(int id)
        {
            var discount = _discountService.GetAll().FirstOrDefault(d => d.Id == id);
            if (discount == null) return NotFound();
            return View(discount);
        }

        // POST: /Discounts/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _discountService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

