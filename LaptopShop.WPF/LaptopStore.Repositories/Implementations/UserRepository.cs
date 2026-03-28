using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public UserRepository()
        {
            _context = new LaptopShopDbContext();
        }
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
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

        }

        public List<User> GetAll()
        {
            // Thêm Include để lôi các Role đi kèm theo User
            return _context.Users
                .Include(u => u.Roles)
                .ToList();
        }

        public User GetById(int id)
        {
            // Tương tự cho hàm lấy theo ID
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == id);
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
        public void UpdateUserRoles(int userId, List<int> roleIds)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null) return;

            user.Roles.Clear();

            var roles = _context.Roles
                .Where(r => roleIds.Contains(r.RoleId))
                .ToList();

            foreach (var role in roles)
            {
                user.Roles.Add(role);
            }

            _context.SaveChanges();
        }
        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }

   

}
