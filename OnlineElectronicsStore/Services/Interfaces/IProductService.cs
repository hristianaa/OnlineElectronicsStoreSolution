using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Returns all products.
        /// </summary>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Returns a single product by its ID, or null if not found.
        /// </summary>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Searches for products whose name or description contains the given keyword.
        /// </summary>
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        /// <summary>
        /// Adds a new product and returns the created entity (including its new ID).
        /// </summary>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// Updates an existing product; returns true if the update succeeded.
        /// </summary>
        Task<bool> UpdateAsync(Product product);

        /// <summary>
        /// Deletes the product with the given ID; returns true if the delete succeeded.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
