using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartItemsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cartitems/user/5
        [HttpGet("user/{userId}")]
        public IActionResult GetUserCart(int userId)
        {
            var cart = _cartService.GetCartItems(userId);
            if (!cart.Any())
                return NotFound(new { Message = "Cart is empty." });

            return Ok(cart);
        }

        // GET: api/cartitems/5
        [HttpGet("{id}")]
        public IActionResult GetCartItem(int id)
        {
            var item = _cartService.GetCartItemById(id);
            if (item == null)
                return NotFound(new { Message = "Item not found in cart." });

            return Ok(item);
        }

        // POST: api/cartitems
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _cartService.AddToCart(item);
            return Ok(new { Message = "Item added to cart." });
        }

        // PUT: api/cartitems/5
        [HttpPut("{id}")]
        public IActionResult UpdateCartItem(int id, [FromBody] CartItem item)
        {
            if (id != item.Id)
                return BadRequest(new { Message = "ID mismatch." });

            var existing = _cartService.GetCartItemById(id);
            if (existing == null)
                return NotFound(new { Message = "Cart item not found." });

            _cartService.UpdateCartItem(item);
            return Ok(new { Message = "Cart item updated." });
        }

        // DELETE: api/cartitems/5
        [HttpDelete("{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            var existing = _cartService.GetCartItemById(id);
            if (existing == null)
                return NotFound(new { Message = "Item not found in cart." });

            _cartService.RemoveFromCart(id);
            return Ok(new { Message = "Item removed from cart." });
        }

        // DELETE: api/cartitems/clear/5
        [HttpDelete("clear/{userId}")]
        public IActionResult ClearCart(int userId)
        {
            _cartService.ClearCart(userId);
            return Ok(new { Message = "Cart cleared." });
        }
    }
}
