using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Publicly accessible: Get all products
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null || products.Count == 0)
                return NotFound(new { Message = "No products available." });

            return Ok(products);
        }

        // ✅ Get single product by ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            return Ok(product);
        }

        // ✅ Add a new product (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid product data." });

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // ✅ Update product (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest(new { Message = "Product ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                    return NotFound(new { Message = "Product not found." });
                else
                    throw;
            }

            return Ok(new { Message = "Product updated successfully." });
        }

        // ✅ Delete product (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}
