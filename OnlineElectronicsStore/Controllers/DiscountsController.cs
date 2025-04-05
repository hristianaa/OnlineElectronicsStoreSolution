using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

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
    public IActionResult GetAll() => Ok(_discountService.GetAll());

    [HttpGet("code/{code}")]
    public IActionResult GetByCode(string code)
    {
        var discount = _discountService.GetByCode(code);
        return discount != null ? Ok(discount) : NotFound(new { Message = "Discount code not found." });
    }

    [HttpPost]
    public IActionResult Create([FromBody] Discount discount)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        discount.ExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(1), DateTimeKind.Unspecified);
        
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
