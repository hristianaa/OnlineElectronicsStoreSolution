using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using System.Security.Claims;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // 📦 GET: api/orders (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            if (!orders.Any())
                return NotFound(new { Message = "No orders found." });

            return Ok(orders);
        }

        // 📄 GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { Message = "Order not found." });

            return Ok(order);
        }

        // 🧾 GET: api/orders/history
        [HttpGet("history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userOrders = await _orderService.GetByUserIdAsync(userId);

            if (!userOrders.Any())
                return NotFound(new { Message = "No orders found for your account." });

            return Ok(userOrders);
        }

        // 🛒 POST: api/orders
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if (order == null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid order data." });

            await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // ❌ DELETE: api/orders/{id} (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var existing = await _orderService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Order not found." });

            await _orderService.DeleteAsync(id);
            return Ok(new { Message = "Order deleted successfully." });
        }
    }
}

