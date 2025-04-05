using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShippingDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // 📦 GET: api/shippingdetails
        [HttpGet]
        public async Task<IActionResult> GetShippingDetails()
        {
            var details = await _context.ShippingDetails.ToListAsync();
            return Ok(details);
        }

        // 📦 GET: api/shippingdetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingDetail(int id)
        {
            var detail = await _context.ShippingDetails.FindAsync(id);
            if (detail == null)
                return NotFound(new { Message = "Shipping detail not found." });

            return Ok(detail);
        }

        // 📦 POST: api/shippingdetails
        [HttpPost]
        public async Task<IActionResult> PostShippingDetail([FromBody] ShippingDetails detail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ShippingDetails.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShippingDetail), new { id = detail.Id }, detail);
        }

        // 📦 PUT: api/shippingdetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippingDetail(int id, [FromBody] ShippingDetails detail)
        {
            if (id != detail.Id)
                return BadRequest(new { Message = "Shipping detail ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ShippingDetails.AnyAsync(e => e.Id == id))
                    return NotFound(new { Message = "Shipping detail not found." });

                throw;
            }

            return Ok(new { Message = "Shipping detail updated successfully." });
        }

        // ❌ DELETE: api/shippingdetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingDetail(int id)
        {
            var detail = await _context.ShippingDetails.FindAsync(id);
            if (detail == null)
                return NotFound(new { Message = "Shipping detail not found." });

            _context.ShippingDetails.Remove(detail);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Shipping detail deleted successfully." });
        }
    }
}
