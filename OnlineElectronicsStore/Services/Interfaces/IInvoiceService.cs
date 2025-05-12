// Services/Interfaces/IInvoiceService.cs
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IInvoiceService
    {
        /// <summary>
        /// Returns all invoices, including their orders, line-items and products.
        /// </summary>
        Task<IEnumerable<Invoice>> GetAllAsync();

        /// <summary>
        /// Returns a single invoice by ID (or null if not found),
        /// including its order details.
        /// </summary>
        Task<Invoice?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new invoice record and returns it (with its new ID).
        /// </summary>
        Task<Invoice> CreateAsync(Invoice invoice);

        /// <summary>
        /// Deletes the invoice with the given ID. Returns true if it existed.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
