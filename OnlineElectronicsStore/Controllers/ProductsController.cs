using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using System.Threading.Tasks;
using System.Linq;

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

        /// <summary>
        /// Get all products (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            if (products == null || products.Count == 0)
                return NotFound(new { Message = "No products available." });

            return Ok(products);
        }

        /// <summary>
        /// Get product by ID (public).
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            return Ok(product);
        }

        /// <summary>
        /// Create a new product (admin only).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid product data." });

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update product (admin only).
        /// </summary>
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
                if (!await _context.Products.AnyAsync(p => p.Id == id))
                    return NotFound(new { Message = "Product not found." });

                throw;
            }

            return Ok(new { Message = "Product updated successfully." });
        }

        /// <summary>
        /// Delete product (admin only).
        /// </summary>
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
