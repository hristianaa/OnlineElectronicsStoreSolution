using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public IActionResult GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest(new { Message = "Invalid pagination values." });

            var products = _productService.GetAllProducts()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!products.Any())
                return NotFound(new { Message = "No products found." });

            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid product data." });

            _productService.AddProduct(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest(new { Message = "Product ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _productService.GetById(id);
            if (existing == null)
                return NotFound(new { Message = "Product not found." });

            _productService.UpdateProduct(product);
            return Ok(new { Message = "Product updated successfully." });
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var existing = _productService.GetById(id);
            if (existing == null)
                return NotFound(new { Message = "Product not found." });

            _productService.DeleteProduct(id);
            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}

