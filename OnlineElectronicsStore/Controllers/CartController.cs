using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]  // only logged‐in users can view or modify their cart
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);  // Views/Cart/Index.cshtml expects a CartDto model
        }

        // POST: /Cart/Add
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var item = new CartItemDto
            {
                ProductId = productId,
                Quantity = quantity
            };
            await _cartService.AddToCartAsync(item);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Remove
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var item = new CartItemDto
            {
                ProductId = productId
            };
            await _cartService.RemoveFromCartAsync(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
