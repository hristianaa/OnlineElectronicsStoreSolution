using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class CartItemsController : Controller
    {
        private readonly ICartService _cartService;
        public CartItemsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /CartItems
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var items = _cartService.GetCartItems(userId).ToList();
            return View(items);
        }

        // GET: /CartItems/Details/5
        public IActionResult Details(int id)
        {
            var item = _cartService.GetCartItemById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /CartItems/Create
        public IActionResult Create(int? productId)
        {
            var model = new CartItem
            {
                ProductId = productId ?? 0,
                Quantity = 1
            };
            return View(model);
        }

        // POST: /CartItems/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CartItem model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = GetCurrentUserId();
            _cartService.AddToCart(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /CartItems/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _cartService.GetCartItemById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /CartItems/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CartItem model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _cartService.UpdateCartItem(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /CartItems/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _cartService.GetCartItemById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /CartItems/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: /CartItems/Clear
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _cartService.ClearCart(GetCurrentUserId());
            return RedirectToAction(nameof(Index));
        }

        // Helper to get the logged-in user’s ID
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new InvalidOperationException("User ID claim missing");
            return int.Parse(claim);
        }
    }
}
