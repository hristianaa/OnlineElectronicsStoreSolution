using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;
using OnlineElectronicsStore.DTOs;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // GET: /Checkout
        public async Task<IActionResult> Index()
        {
            // Optionally, get a summary (cart total, items, etc.)
            // var summary = await _checkoutService.GetCheckoutSummaryAsync(GetCurrentUserId());
            // return View(summary);

            return View();  // Views/Checkout/Index.cshtml
        }

        // POST: /Checkout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutDto model)
        {
            if (!ModelState.IsValid)
            {
                // re-display the form with validation errors
                return View(model);
            }

            var userId = GetCurrentUserId();
            bool success;
            try
            {
                success = await _checkoutService.PlaceOrderAsync(userId);
            }
            catch (Exception ex)
            {
                // log ex as needed
                ModelState.AddModelError("", "An error occurred while placing your order.");
                return View(model);
            }

            if (success)
                return RedirectToAction(nameof(Confirmation));

            ModelState.AddModelError("", "Failed to place the order. Please try again.");
            return View(model);
        }

        // GET: /Checkout/Confirmation
        public IActionResult Confirmation()
        {
            return View(); // Views/Checkout/Confirmation.cshtml
        }

        private int GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(id, out var userId) ? userId : throw new InvalidOperationException("User ID missing");
        }
    }
}

