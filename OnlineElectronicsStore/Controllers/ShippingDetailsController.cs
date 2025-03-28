using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShippingDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingDetails>>> GetShippingDetails()
        {
            return await _context.ShippingDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingDetails>> GetShippingDetail(int id)
        {
            var shippingDetail = await _context.ShippingDetails.FindAsync(id);
            if (shippingDetail == null) return NotFound();
            return shippingDetail;
        }

        [HttpPost]
        public async Task<ActionResult<ShippingDetails>> PostShippingDetail(ShippingDetails shippingDetail)
        {
            _context.ShippingDetails.Add(shippingDetail);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShippingDetail), new { id = shippingDetail.Id }, shippingDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippingDetail(int id, ShippingDetails shippingDetail)
        {
            if (id != shippingDetail.Id) return BadRequest();

            _context.Entry(shippingDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingDetail(int id)
        {
            var shippingDetail = await _context.ShippingDetails.FindAsync(id);
            if (shippingDetail == null) return NotFound();

            _context.ShippingDetails.Remove(shippingDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
