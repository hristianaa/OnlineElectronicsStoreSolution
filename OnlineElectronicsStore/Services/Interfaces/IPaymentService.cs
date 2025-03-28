using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetAll();
        Payment? GetById(int id);
        void Create(Payment payment);
        void MarkAsPaid(int id);
        void Delete(int id);
    }
}
