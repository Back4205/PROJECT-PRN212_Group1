using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Implementations
{
    public  class UserRepository : IUserRepository
    {
        private readonly LaptopShopDbContext _context;
=======
using Microsoft.EntityFrameworkCore;
namespace LaptopShop.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly LaptopShopDbContext _context;

>>>>>>> origin/Qui
        public UserRepository()
        {
            _context = new LaptopShopDbContext();
        }
<<<<<<< HEAD
        // viết Cac lau lenh truy van trong cac class nay .
        public void Add(User user)
        {
           _context.Users.Add(user);
            _context.SaveChanges();
            AddCustomer(user.UserId);
        }

        public void AddCustomer(int userId)
        {
            var customer = new Customer
            {
                UserId = userId,
                Address = "Unknown"
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void AddUserRole(int userId, int roleId)
        {
            var user = _context.Users
                               .Where(u => u.UserId == userId)
                               .FirstOrDefault();

            var role = _context.Roles
                               .Where(r => r.RoleId == roleId)
                               .FirstOrDefault();

            if (user != null && role != null)
            {
                user.Roles.Add(role);
                _context.SaveChanges();
            }
=======

        public List<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Roles)
                .OrderByDescending(u => u.CreatedAt)
                .ToList();
        }

        public User GetById(int id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Username == username);
        }

        public User GetByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == email);
        }

        public List<Role> GetAllRoles()
        {
            return _context.Roles
                .OrderBy(r => r.RoleName)
                .ToList();
        }

        public int CountActiveAdmins()
        {
            return _context.Users
                .Include(u => u.Roles)
                .Count(u => u.IsActive && u.Roles.Any(r => r.RoleName == "Admin"));
        }

        public bool UserHasRole(int userId, string roleName)
        {
            return _context.Users
                .Include(u => u.Roles)
                .Any(u => u.UserId == userId && u.Roles.Any(r => r.RoleName == roleName));
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
>>>>>>> origin/Qui
        }

        public void Delete(int id)
        {
<<<<<<< HEAD
            var user = _context.Users.Find(id);
=======
            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == id);

>>>>>>> origin/Qui
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
<<<<<<< HEAD

        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<Role> GetRolesByUserId(int userId)
        {
            return _context.Users
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Roles)
                .ToList();
        }

        public bool IsEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsUsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public void Update(User user)
        {
            var existingUser = _context.Users.FirstOrDefault((u => u.UserId == user.UserId));
            if (existingUser != null) {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
        }
        public Customer GetCustomerByUserId(int userId)
        {
          
            return _context.Customers.FirstOrDefault(c => c.UserId == userId);
        }
        // Trong UserRepository.cs
        public void UpdateCustomer(Customer customer)
        {
            var existing = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(customer);
                _context.SaveChanges();
            }
        }
    }

   

}
=======
        }
    }
}
>>>>>>> origin/Qui
