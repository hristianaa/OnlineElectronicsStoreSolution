using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: /Payments
        public IActionResult Index()
        {
            var payments = _paymentService.GetAll().ToList();
            return View(payments);
        }

        // GET: /Payments/Details/5
        public IActionResult Details(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // GET: /Payments/Create
        public IActionResult Create()
        {
            return View(new Payment());
        }

        // POST: /Payments/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Payment model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _paymentService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Payments/MarkPaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkPaid(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null) return NotFound();

            _paymentService.MarkAsPaid(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Payments/Delete/5
        public IActionResult Delete(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: /Payments/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _paymentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
