using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Get all orders (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return orders.Any() ? Ok(orders) : NotFound(new { Message = "No orders found." });
        }

        /// <summary>
        /// Get a single order by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return order != null ? Ok(order) : NotFound(new { Message = "Order not found." });
        }

        /// <summary>
        /// Get the authenticated user's order history.
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var orders = await _orderService.GetByUserIdAsync(userId);
            return orders.Any() ? Ok(orders) : NotFound(new { Message = "No orders found for your account." });
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            order.OrderDate = DateTime.UtcNow;
            await _orderService.CreateAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Delete an order by ID (Admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { Message = "Order not found." });

            await _orderService.DeleteAsync(id);
            return Ok(new { Message = "Order deleted successfully." });
        }
    }
}
