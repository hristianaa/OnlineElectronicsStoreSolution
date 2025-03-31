using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // Get all products, including the category information
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // Get a product by its ID, including its category
        public async Task<Product?> GetById(int id)
        {
            var product = await _context.Products
                                         .Include(p => p.Category)
                                         .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                // Optionally log this error or throw a custom exception
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return product;
        }

        // Search for products by keyword (name or description)
        public async Task<IEnumerable<Product>> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Keyword cannot be null or empty.", nameof(keyword));
            }

            return await _context.Products
                                 .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // Add a new product to the database
        public async Task AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            // Optional: You can validate the product before saving
            ValidateProduct(product);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Update an existing product
        public async Task UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            var existingProduct = await _context.Products
                                                 .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            }

            // Update the existing product's properties
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
        }

        // Delete a product from the database
        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        // Validate product properties
        private void ValidateProduct(Product product)
        {
            var validationContext = new ValidationContext(product);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(product, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                throw new InvalidOperationException($"Product is invalid: {errors}");
            }
        }
    }
}
