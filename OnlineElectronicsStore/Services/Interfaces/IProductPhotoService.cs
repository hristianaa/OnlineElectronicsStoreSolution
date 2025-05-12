using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IProductPhotoService
    {
        /// <summary>
        /// Adds a photo URL for the given product.
        /// </summary>
        Task AddPhotoAsync(int productId, string url);

        /// <summary>
        /// Deletes a photo by its ID; returns true if successful.
        /// </summary>
        Task<bool> DeletePhotoAsync(int photoId);
    }
}
