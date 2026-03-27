using LaptopShop.Entities.Models;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> origin/Qui

namespace LaptopShop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsername(string username);
<<<<<<< HEAD

        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);

        void Add(User user);
        void AddUserRole(int userId, int roleId);
        void AddCustomer(int userId);

        void Update(User user);
        void Delete(int id);
        List<Role> GetRolesByUserId(int userId);
        Customer GetCustomerByUserId(int userId);
        void UpdateCustomer(Customer customer);
    }
}
=======
        User GetByEmail(string email);
        List<Role> GetAllRoles();
        int CountActiveAdmins();
        bool UserHasRole(int userId, string roleName);

        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
>>>>>>> origin/Qui
