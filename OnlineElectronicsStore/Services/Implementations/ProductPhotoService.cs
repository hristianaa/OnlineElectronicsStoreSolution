using System.Threading.Tasks;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class ProductPhotoService : IProductPhotoService
    {
        private readonly AppDbContext _context;
        public ProductPhotoService(AppDbContext context) => _context = context;

        public async Task AddPhotoAsync(int productId, string url)
        {
            var photo = new ProductPhoto { ProductId = productId, Url = url };
            _context.ProductPhotos.Add(photo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePhotoAsync(int photoId)
        {
            var photo = await _context.ProductPhotos.FindAsync(photoId);
            if (photo == null) return false;
            _context.ProductPhotos.Remove(photo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
