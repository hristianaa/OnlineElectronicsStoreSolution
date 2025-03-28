using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _orderService.GetAll();
            if (!orders.Any())
                return NotFound(new { Message = "No orders found." });

            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound(new { Message = "Order not found." });

            return Ok(order);
        }

        // GET: api/orders/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUser(int userId)
        {
            var userOrders = _orderService.GetByUserId(userId);
            if (!userOrders.Any())
                return NotFound(new { Message = "No orders found for this user." });

            return Ok(userOrders);
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult PostOrder([FromBody] Order order)
        {
            if (order == null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid order data." });

            _orderService.Create(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var existing = _orderService.GetById(id);
            if (existing == null)
                return NotFound(new { Message = "Order not found." });

            _orderService.Delete(id);
            return Ok(new { Message = "Order deleted successfully." });
        }
    }
}
