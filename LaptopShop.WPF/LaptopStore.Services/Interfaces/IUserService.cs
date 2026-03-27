using LaptopShop.Entities.Models;

namespace LaptopShop.Services.Interfaces
{
    public interface IUserService
    {
        User Login(string username, string password);
        void Register(User user);

        List<User> GetAllUsers();
        List<Role> GetAllRoles();
        void UpdateUserRoles(int userId, List<int> roleIds);
        void SetUserActiveStatus(int userId, bool isActive);

        void AddUserByAdmin(User user, List<int> roleIds);
    }
}