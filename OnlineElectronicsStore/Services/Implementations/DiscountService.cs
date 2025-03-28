using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly AppDbContext _context;

        public DiscountService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Discount> GetAll()
        {
            return _context.Discounts.ToList();
        }

        public Discount? GetByCode(string code)
        {
            return _context.Discounts.FirstOrDefault(d => d.DiscountCode == code);
        }

        public void Create(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var discount = _context.Discounts.Find(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                _context.SaveChanges();
            }
        }
    }
}
