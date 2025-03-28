using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IDiscountService
    {
        IEnumerable<Discount> GetAll();
        Discount? GetByCode(string code);
        void Create(Discount discount);
        void Delete(int id);
    }
}
