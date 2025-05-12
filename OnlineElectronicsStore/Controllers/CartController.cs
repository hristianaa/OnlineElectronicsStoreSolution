using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [AllowAnonymous]
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
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var items = await _cart.GetCartItemsAsync(userId);
                return View(items);
            }

            ViewBag.Message = "Your cart is empty. Log in to save your items.";
            return View(Enumerable.Empty<CartItem>());
        }

        // GET /Cart/Add/5
        [HttpGet("Add/{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            var prod = await _products.GetByIdAsync(productId);
            if (prod == null) return NotFound();

            ViewBag.Product = prod;
            return View(new CartItem { ProductId = productId, Quantity = 1 });
        }

        // POST /Cart/Add
        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CartItem model)
        {
            if (!ModelState.IsValid)
            {
                var prod = await _products.GetByIdAsync(model.ProductId);
                ViewBag.Product = prod;
                return View(model);
            }

            // if anonymous, you could redirect to login; for now we force login
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Cart" });

            model.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cart.AddCartItemAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET /Cart/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _cart.GetCartItemByIdAsync(id);
            if (item == null) return NotFound();

            ViewBag.Products = new SelectList(
                await _products.GetAllAsync(),
                "Id", "Name",
                item.ProductId);

            return View(item);
        }

        // POST /Cart/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CartItem model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Products = new SelectList(
                    await _products.GetAllAsync(),
                    "Id", "Name",
                    model.ProductId);

                return View(model);
            }

            await _cart.UpdateCartItemAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET /Cart/Remove/5
        [HttpGet("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _cart.GetCartItemByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST /Cart/Remove/5
        [HttpPost("Remove/{id}"), ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            await _cart.RemoveCartItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
