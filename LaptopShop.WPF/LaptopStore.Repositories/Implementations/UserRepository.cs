using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LaptopShop.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly LaptopShopDbContext _context;

        public UserRepository()
        {
            _context = new LaptopShopDbContext();
        }

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
        }

        public void Delete(int id)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}