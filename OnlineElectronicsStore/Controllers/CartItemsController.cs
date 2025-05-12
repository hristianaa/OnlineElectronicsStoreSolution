using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CartItemsController : Controller
    {
        private readonly ICartService _cart;
        private readonly IProductService _products;

        public CartItemsController(ICartService cartService, IProductService productService)
        {
            _cart = cartService;
            _products = productService;
        }

        // GET /CartItems
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var items = await _cart.GetCartItemsAsync(userId);
            return View(items);
        }

        // GET /CartItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _cart.GetCartItemByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET /CartItems/Create
        public async Task<IActionResult> Create()
        {
            var prods = await _products.GetAllAsync();
            ViewBag.Products = new SelectList(prods, "Id", "Name");
            return View(new CartItem());
        }

        // POST /CartItems/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartItem model)
        {
            if (!ModelState.IsValid)
            {
                var prods = await _products.GetAllAsync();
                ViewBag.Products = new SelectList(prods, "Id", "Name");
                return View(model);
            }

            model.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cart.AddCartItemAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET /CartItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _cart.GetCartItemByIdAsync(id);
            if (item == null) return NotFound();

            var prods = await _products.GetAllAsync();
            ViewBag.Products = new SelectList(prods, "Id", "Name", item.ProductId);
            return View(item);
        }

        // POST /CartItems/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CartItem model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var prods = await _products.GetAllAsync();
                ViewBag.Products = new SelectList(prods, "Id", "Name", model.ProductId);
                return View(model);
            }

            await _cart.UpdateCartItemAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET /CartItems/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _cart.GetCartItemByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST /CartItems/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cart.RemoveCartItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
