using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Returns all categories.
        /// </summary>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Gets a single category by ID, or null if not found.
        /// </summary>
        Task<Category?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new category.
        /// </summary>
        Task<Category> AddAsync(Category category);

        /// <summary>
        /// Updates an existing category; returns true if successful.
        /// </summary>
        Task<bool> UpdateAsync(Category category);

        /// <summary>
        /// Deletes a category; returns true if successful.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
