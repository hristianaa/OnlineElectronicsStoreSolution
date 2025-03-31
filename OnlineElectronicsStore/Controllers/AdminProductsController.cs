using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/products")]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminProductsController(AppDbContext context)
        {
            _context = context;
        }

        // 🔍 GET: List all products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return Ok(products);
        }

        // ➕ POST: Add new product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }

        // ✏️ PUT: Edit product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.Price = updatedProduct.Price;
            existing.Stock = updatedProduct.Stock;
            existing.CategoryId = updatedProduct.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // ❌ DELETE: Delete product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product deleted." });
        }
    }
}
