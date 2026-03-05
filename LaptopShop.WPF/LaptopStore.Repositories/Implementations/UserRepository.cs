using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
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
        }

        public void AddCustomer(int userId)
        {
            var customer = new Customer
            {
                UserId = userId,
                Address = "Chưa cập nhật"
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
    }

       
    
}
