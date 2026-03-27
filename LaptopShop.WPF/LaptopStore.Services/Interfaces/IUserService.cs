using LaptopShop.Entities.Models;

namespace LaptopShop.Services.Interfaces
{
    public interface IUserService
    {
        User Login(string username, string password);
        void Register(User user);
        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);
        List<Role> GetRolesByUserId(int userId);
        User GetUserById(int id);
        void UpdateUserProfile(User user, string newPassword, string address = null);
        Customer GetCustomerByUserId(int userId);

        List<User> GetAllUsers();
        List<Role> GetAllRoles();
        void UpdateUserRoles(int userId, List<int> roleIds);
        void SetUserActiveStatus(int userId, bool isActive);
        void AddUserByAdmin(User user, List<int> roleIds);
    }
}