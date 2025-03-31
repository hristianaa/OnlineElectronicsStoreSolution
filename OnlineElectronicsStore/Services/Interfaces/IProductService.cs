
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();  // Get all products
        Task<Product?> GetById(int id);               // Get a product by ID
        Task<IEnumerable<Product>> Search(string keyword);  // Search products by keyword
        Task AddProduct(Product product);  // Add a new product
        Task UpdateProduct(Product product);  // Update an existing product
        Task DeleteProduct(int id);  // Delete a product by ID
    }
}
