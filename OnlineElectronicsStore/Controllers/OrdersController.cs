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
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orders;
        public OrdersController(IOrderService orders) => _orders = orders;

        // GET /Orders/History
        [HttpGet("History")]
        public async Task<IActionResult> History()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dtos = await _orders.GetOrderHistoryAsync(userId);

            var vm = dtos
                .Select(o => new OrderHistoryViewModel
                {
                    Id = o.Id,
                    CreatedOn = o.OrderDate,
                    Total = o.Items.Sum(i => i.Quantity * i.UnitPrice),
                    Status = o.Status
                })
                .ToList();

            return View(vm);  // Views/Orders/History.cshtml @model List<OrderHistoryViewModel>
        }

        // GET /Orders/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dto = await _orders.GetOrderByIdAsync(userId, id);
            if (dto == null) return NotFound();

            var vm = new OrderDetailsViewModel
            {
                Id = dto.Id,
                CreatedOn = dto.OrderDate,
                Status = dto.Status,
                Total = dto.Items.Sum(i => i.Quantity * i.UnitPrice),
                Items = dto.Items
                              .Select(i => new OrderItemViewModel
                              {
                                  ProductName = i.ProductName,
                                  Quantity = i.Quantity,
                                  UnitPrice = i.UnitPrice
                              })
                              .ToList()
            };

            return View(vm);  // Views/Orders/Details.cshtml @model OrderDetailsViewModel
        }

        // POST /Orders/Delete/{id}
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _orders.DeleteOrderAsync(userId, id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(History));
        }
    }
}
