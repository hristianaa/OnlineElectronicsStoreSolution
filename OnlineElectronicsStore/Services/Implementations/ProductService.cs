using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .Include(p => p.Photos)
                                 .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .Include(p => p.Photos)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Enumerable.Empty<Product>();

            return await _context.Products
                                 .Where(p => p.Name.Contains(keyword)
                                          || p.ShortDescription.Contains(keyword)
                                          || p.LongDescription.Contains(keyword))
                                 .Include(p => p.Category)
                                 .Include(p => p.Photos)
                                 .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            ValidateProduct(product);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existing = await _context.Products
                                         .Include(p => p.Photos)
                                         .FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existing == null)
                return false;

            ValidateProduct(product);

            // copy over scalar and navigation props
            _context.Entry(existing).CurrentValues.SetValues(product);

            // replace Photos collection if needed
            existing.Photos.Clear();
            foreach (var photo in product.Photos)
                existing.Photos.Add(photo);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private void ValidateProduct(Product product)
        {
            var ctx = new ValidationContext(product);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(product, ctx, results, true))
            {
                var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new InvalidOperationException($"Product validation failed: {errors}");
            }
        }
    }
}

