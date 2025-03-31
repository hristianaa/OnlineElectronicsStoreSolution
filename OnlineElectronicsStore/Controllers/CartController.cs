using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;
using OnlineElectronicsStore.DTOs;


namespace OnlineElectronicsStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto item)
        {
            var result = await _cartService.AddToCartAsync(item);
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartItemDto item)
        {
            var result = await _cartService.RemoveFromCartAsync(item);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCartAsync();
            return Ok(cart);
        }
    }

}
