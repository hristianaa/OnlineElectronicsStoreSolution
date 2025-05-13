// Controllers/OrdersController.cs
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models.ViewModels;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orders;
        public OrdersController(IOrderService orders)
            => _orders = orders;

        // GET /Orders/History
        [HttpGet("History")]
        public async Task<IActionResult> History()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // DTO-based call
            var dtos = await _orders.GetOrderHistoryAsync(userId);

            // Map OrderDto → OrderHistoryViewModel
            var vm = dtos
                .Select(o => new OrderHistoryViewModel
                {
                    Id = o.Id,
                    Date = o.OrderDate,
                    Total = o.Items.Sum(i => i.Quantity * i.UnitPrice),
                    Status = o.Status
                })
                .ToList();

            return View(vm);  // Views/Orders/History.cshtml @model IEnumerable<OrderHistoryViewModel>
        }

        // GET /Orders/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // DTO-based call
            var dto = await _orders.GetOrderByIdAsync(userId, id);
            if (dto == null) return NotFound();

            // Map OrderDto → OrderDetailsViewModel
            var vm = new OrderDetailsViewModel
            {
                Id = dto.Id,
                CreatedOn = dto.OrderDate,
                Status = dto.Status,
                Items = dto.Items
                              .Select(i => new OrderItemViewModel
                              {
                                  ProductName = i.ProductName,
                                  Quantity = i.Quantity,
                                  UnitPrice = i.UnitPrice
                              })
                              .ToList(),
                Total = dto.Items.Sum(i => i.Quantity * i.UnitPrice)
            };

            return View(vm);  // Views/Orders/Details.cshtml @model OrderDetailsViewModel
        }

        // POST /Orders/Delete/{id}
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // DTO-based call
            var success = await _orders.DeleteOrderAsync(userId, id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(History));
        }
    }
}
