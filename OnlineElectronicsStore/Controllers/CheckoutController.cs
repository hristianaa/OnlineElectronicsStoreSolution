using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models.ViewModels;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly ICheckoutService _checkout;
        private readonly AppDbContext _db;

        public CheckoutController(
            ICartService cart,
            ICheckoutService checkout,
            AppDbContext db)
        {
            _cart = cart;
            _checkout = checkout;
            _db = db;
        }

        // GET /Checkout
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cart.GetCartDtoAsync(userId);

            var vm = new CheckoutViewModel
            {
                Items = cart.Items,
                Subtotal = cart.TotalPrice,
                ShippingFee = cart.Items.Any() ? 5m : 0m,
                Total = cart.TotalPrice + (cart.Items.Any() ? 5m : 0m)
            };
            return View(vm);
        }

        // POST /Checkout (apply code or place order)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel vm, string action)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cart.GetCartDtoAsync(userId);

            // rehydrate totals & items
            vm.Items = cart.Items;
            vm.Subtotal = cart.TotalPrice;
            vm.ShippingFee = cart.Items.Any() ? 5m : 0m;
            vm.Total = vm.Subtotal + vm.ShippingFee;
            vm.DiscountApplied = 0;
            vm.DiscountMessage = null;

            if (action == "Apply")  // clicked “Apply”
            {
                if (!string.IsNullOrWhiteSpace(vm.DiscountCode))
                {
                    var now = DateTime.UtcNow;
                    var disc = await _db.Discounts
                        .Where(d => d.DiscountCode == vm.DiscountCode
                                 && d.ExpiryDate >= now)
                        .FirstOrDefaultAsync();

                    if (disc == null)
                    {
                        vm.DiscountMessage = "Invalid or expired code.";
                    }
                    else if (!cart.Items.Any(i => i.ProductId == disc.ProductId))
                    {
                        vm.DiscountMessage = "Code not valid for items in your cart.";
                    }
                    else
                    {
                        vm.DiscountApplied = disc.DiscountAmount;
                    }
                }
                return View(vm);
            }
            else if (action == "Confirm")  // clicked “Confirm and Pay”
            {
                if (!ModelState.IsValid)
                    return View(vm);

                // carry discount through to order
                vm.DiscountApplied = decimal.Round(vm.DiscountApplied, 2);
                int orderId = await _checkout.PlaceOrderAsync(userId, vm);
                return RedirectToAction(nameof(Confirmation), new { id = orderId });
            }

            // fallback
            return View(vm);
        }

        // GET /Checkout/Confirmation/5
        [HttpGet]
        public IActionResult Confirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
