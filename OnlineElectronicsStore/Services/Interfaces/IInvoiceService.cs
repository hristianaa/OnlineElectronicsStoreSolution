using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetAll();
        Invoice? GetById(int id);
        void Create(Invoice invoice);
        void Delete(int id);
    }
}
