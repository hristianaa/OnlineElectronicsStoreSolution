using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _payments;

        public PaymentsController(IPaymentService paymentService)
        {
            _payments = paymentService;
        }

        // GET /Payments
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var list = await _payments.GetAllAsync();
            return View(list);
        }

        // GET /Payments/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var p = await _payments.GetByIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        // GET /Payments/Create
        public IActionResult Create() => View(new Payment());

        // POST /Payments/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment model)
        {
            if (!ModelState.IsValid) return View(model);
            await _payments.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // POST /Payments/MarkPaid/{id}
        [HttpPost]
        public async Task<IActionResult> MarkPaid(int id)
        {
            await _payments.MarkAsPaidAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST /Payments/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _payments.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
