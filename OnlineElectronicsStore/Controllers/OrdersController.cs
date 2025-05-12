using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: /Orders
        // Admin only: list all orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            if (!orders.Any())
                ViewBag.Message = "No orders found.";
            return View(orders);
        }

        // GET: /Orders/Details/5
        // Anyone authorized can view a single order (you could restrict to Admin or owner)
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /Orders/History
        // Show the logged‐in user’s order history
        public async Task<IActionResult> History()
        {
            var userId = GetCurrentUserId();
            var userOrders = await _orderService.GetByUserIdAsync(userId);
            if (!userOrders.Any())
                ViewBag.Message = "You have no past orders.";
            return View(userOrders);
        }

        // GET: /Orders/Delete/5
        // Admin only: confirm deletion
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /Orders/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new InvalidOperationException("User ID claim missing");
            return int.Parse(claim);
        }
    }
}
