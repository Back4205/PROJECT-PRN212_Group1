using LaptopShop.Entities.Models;

namespace LaptopShop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsername(string username);
        User GetByEmail(string email);
        List<Role> GetAllRoles();
        int CountActiveAdmins();
        bool UserHasRole(int userId, string roleName);

        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}