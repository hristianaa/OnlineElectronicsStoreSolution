// Services/Interfaces/IDiscountService.cs
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IDiscountService
    {
        /// <summary>
        /// Returns all discounts.
        /// </summary>
        Task<IEnumerable<Discount>> GetAllAsync();

        /// <summary>
        /// Finds a discount by its code, or null if none.
        /// </summary>
        Task<Discount?> GetByCodeAsync(string code);

        /// <summary>
        /// Creates a new discount and returns the created entity (with ID).
        /// </summary>
        Task<Discount> CreateAsync(Discount discount);

        /// <summary>
        /// Deletes the discount with the given ID. Returns true if deleted.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
