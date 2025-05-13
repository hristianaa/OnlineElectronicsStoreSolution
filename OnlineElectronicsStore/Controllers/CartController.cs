using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        private readonly IProductService _products;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cart = cartService;
            _products = productService;
        }

        // GET /Cart
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _cart.GetCartDtoAsync(userId);
            return View(cart);
        }

        // POST /Cart/Add/5
        [HttpPost("Add/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            // ensure product exists
            var prod = await _products.GetByIdAsync(productId);
            if (prod == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = new CartItemDto
            {
                ProductId = productId,
                Quantity = quantity,
                ProductName = prod.Name,
                UnitPrice = prod.Price
            };

            await _cart.AddToCartAsync(userId, item);
            return RedirectToAction(nameof(Index));
        }

        // POST /Cart/Remove/5
        [HttpPost("Remove/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = new CartItemDto { ProductId = productId, Quantity = 0 };

            await _cart.RemoveFromCartAsync(userId, item);
            return RedirectToAction(nameof(Index));
        }
    }
}
