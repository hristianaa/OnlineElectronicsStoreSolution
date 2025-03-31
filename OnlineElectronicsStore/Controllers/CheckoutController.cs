using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var result = await _checkoutService.PlaceOrderAsync(1); // TEMP userId
            if (result)
                return Ok("Order placed successfully.");
            else
                return BadRequest("Failed to place order.");
        }
    }
}
