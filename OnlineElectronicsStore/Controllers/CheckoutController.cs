using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkout;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkout = checkoutService;
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            return View();  // your cart/checkout summary view
        }

        // POST: /Checkout/PlaceOrder
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _checkout.PlaceOrderAsync(userId);
            if (success)
                return RedirectToAction("OrderConfirmation");
            else
            {
                TempData["Error"] = "Your cart was empty.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Checkout/OrderConfirmation
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
