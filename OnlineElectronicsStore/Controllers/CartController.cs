using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cartService)
        {
            _cart = cartService;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartDto = await _cart.GetCartDtoAsync(userId);
            return View(cartDto);
        }

        // POST: /Cart/Add
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _cart.AddToCartAsync(userId, new CartItemDto
            {
                ProductId = productId,
                Quantity = quantity
            });

            if (!success)
                TempData["Error"] = "Could not add product to cart.";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Remove
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cart.RemoveFromCartAsync(userId, new CartItemDto
            {
                ProductId = productId,
                Quantity = 0
            });
            return RedirectToAction(nameof(Index));
        }
    }
}
