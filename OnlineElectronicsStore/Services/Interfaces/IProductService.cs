using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetById(int id);
        Task<IEnumerable<Product>> Search(string keyword);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
