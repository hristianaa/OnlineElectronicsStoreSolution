using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models.ViewModels;
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

            var vm = orders.Select(o => new OrderListItemViewModel
            {
                Id = o.Id,
                CreatedOn = o.OrderDate,           // ← use OrderDate
                Total = o.TotalAmount,         // ← use TotalAmount
                Status = o.Status
            })
            .ToList();

            if (!vm.Any())
                ViewBag.Message = "No orders found.";

            return View(vm);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /Orders/History
        public async Task<IActionResult> History()
        {
            var userId = GetCurrentUserId();
            var userOrders = await _orderService.GetByUserIdAsync(userId);

            var vm = userOrders.Select(o => new OrderListItemViewModel
            {
                Id = o.Id,
                CreatedOn = o.OrderDate,         // ← OrderDate
                Total = o.TotalAmount,       // ← TotalAmount
                Status = o.Status
            })
            .ToList();

            if (!vm.Any())
                ViewBag.Message = "You have no past orders.";

            return View(vm);
        }

        // GET: /Orders/Delete/5
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
