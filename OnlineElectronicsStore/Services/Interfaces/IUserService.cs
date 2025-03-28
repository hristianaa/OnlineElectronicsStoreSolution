using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User? GetByEmail(string email);
        void Create(User user);
        void Update(User user);
        void Delete(int id);
    }
}
