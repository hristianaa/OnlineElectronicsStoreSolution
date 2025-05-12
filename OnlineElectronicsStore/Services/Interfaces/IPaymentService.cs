// Services/Interfaces/IPaymentService.cs
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Returns all payments.
        /// </summary>
        Task<IEnumerable<Payment>> GetAllAsync();

        /// <summary>
        /// Returns a single payment by ID, or null if not found.
        /// </summary>
        Task<Payment?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new payment record.
        /// </summary>
        Task<Payment> CreateAsync(Payment payment);

        /// <summary>
        /// Marks the given payment as paid. Returns true if successful.
        /// </summary>
        Task<bool> MarkAsPaidAsync(int id);

        /// <summary>
        /// Deletes the payment with the given ID. Returns true if it existed.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
