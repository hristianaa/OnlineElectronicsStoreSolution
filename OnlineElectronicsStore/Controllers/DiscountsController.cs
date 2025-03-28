using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_discountService.GetAll());
        }

        [HttpGet("code/{code}")]
        public IActionResult GetByCode(string code)
        {
            var discount = _discountService.GetByCode(code);
            if (discount == null)
                return NotFound(new { Message = "Discount code not found." });

            return Ok(discount);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Discount discount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _discountService.Create(discount);
            return Ok(new { Message = "Discount added." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _discountService.Delete(id);
            return Ok(new { Message = "Discount deleted." });
        }
    }
}
