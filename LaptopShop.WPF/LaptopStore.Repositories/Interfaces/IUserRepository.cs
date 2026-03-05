using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsername(string username);

        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);

        void Add(User user);
        void AddUserRole(int userId, int roleId);
        void AddCustomer(int userId);

        void Update(User user);
        void Delete(int id);
        List<Role> GetRolesByUserId(int userId);
    }
}
