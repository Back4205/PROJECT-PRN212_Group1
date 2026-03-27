using LaptopShop.Entities.Models;
using LaptopShop.Repositories;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LaptopShop.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username và password không được để trống.");
            }

            var user = _userRepository.GetByUsername(username.Trim());

            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại.");
            }

            if (!user.IsActive)
            {
                throw new Exception("Tài khoản đã bị khóa.");
            }

            string hashedPassword = HashPassword(password.Trim());

            if (user.PasswordHash != hashedPassword)
            {
                throw new Exception("Sai mật khẩu.");
            }

            return user;
        }

        public void Register(User user)
        {
            if (user == null)
            {
                throw new Exception("Dữ liệu người dùng không hợp lệ.");
            }

            ValidateUserBasicInfo(user);

            if (_userRepository.GetByUsername(user.Username.Trim()) != null)
            {
                throw new Exception("Username đã tồn tại.");
            }

            if (_userRepository.GetByEmail(user.Email.Trim()) != null)
            {
                throw new Exception("Email đã tồn tại.");
            }

            user.Username = user.Username.Trim();
            user.FullName = user.FullName.Trim();
            user.Email = user.Email.Trim();
            user.Phone = user.Phone.Trim();
            user.PasswordHash = HashPassword(user.PasswordHash);
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;

            _userRepository.Add(user);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public List<Role> GetAllRoles()
        {
            return _userRepository.GetAllRoles();
        }

        public void UpdateUserRoles(int userId, List<int> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                throw new Exception("Người dùng phải có ít nhất 1 role.");
            }

            using var context = new LaptopShopDbContext();

            var userInDb = context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == userId);

            if (userInDb == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }

            bool wasAdmin = userInDb.Roles.Any(r => r.RoleName == "Admin");

            var selectedRoles = context.Roles
                .Where(r => roleIds.Contains(r.RoleId))
                .ToList();

            bool willBeAdmin = selectedRoles.Any(r => r.RoleName == "Admin");

            int activeAdminCount = _userRepository.CountActiveAdmins();

            if (userInDb.IsActive && wasAdmin && !willBeAdmin && activeAdminCount <= 1)
            {
                throw new Exception("Không thể bỏ role Admin của admin cuối cùng.");
            }

            userInDb.Roles.Clear();

            foreach (var role in selectedRoles)
            {
                userInDb.Roles.Add(role);
            }

            context.SaveChanges();
        }

        public void SetUserActiveStatus(int userId, bool isActive)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }

            bool isAdmin = user.Roles.Any(r => r.RoleName == "Admin");
            int activeAdminCount = _userRepository.CountActiveAdmins();

            if (!isActive && user.IsActive && isAdmin && activeAdminCount <= 1)
            {
                throw new Exception("Không thể khóa admin cuối cùng.");
            }

            user.IsActive = isActive;
            _userRepository.Update(user);
        }

        public void DeleteUser(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }

            bool isAdmin = user.Roles.Any(r => r.RoleName == "Admin");
            int activeAdminCount = _userRepository.CountActiveAdmins();

            if (user.IsActive && isAdmin && activeAdminCount <= 1)
            {
                throw new Exception("Không thể xóa admin cuối cùng.");
            }

            _userRepository.Delete(userId);
        }

        public void AddUserByAdmin(User user, List<int> roleIds)
        {
            if (user == null)
            {
                throw new Exception("Dữ liệu user không hợp lệ.");
            }

            ValidateUserBasicInfo(user);

            if (roleIds == null || roleIds.Count == 0)
            {
                throw new Exception("User phải có ít nhất 1 role.");
            }

            if (_userRepository.GetByUsername(user.Username.Trim()) != null)
            {
                throw new Exception("Username đã tồn tại.");
            }

            if (_userRepository.GetByEmail(user.Email.Trim()) != null)
            {
                throw new Exception("Email đã tồn tại.");
            }

            using var context = new LaptopShopDbContext();

            var selectedRoles = context.Roles
                .Where(r => roleIds.Contains(r.RoleId))
                .ToList();

            user.Username = user.Username.Trim();
            user.PasswordHash = HashPassword(user.PasswordHash);
            user.FullName = user.FullName.Trim();
            user.Email = user.Email.Trim();
            user.Phone = user.Phone.Trim();
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;

            foreach (var role in selectedRoles)
            {
                user.Roles.Add(role);
            }

            context.Users.Add(user);
            context.SaveChanges();
        }

        private void ValidateUserBasicInfo(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username không được để trống.");

            if (user.Username.Trim().Length < 3)
                throw new Exception("Username phải có ít nhất 3 ký tự.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new Exception("Password không được để trống.");

            if (user.PasswordHash.Trim().Length < 6)
                throw new Exception("Password phải có ít nhất 6 ký tự.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Full name không được để trống.");

            if (user.FullName.Trim().Length < 2)
                throw new Exception("Full name không hợp lệ.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email không được để trống.");

            string email = user.Email.Trim();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Email không đúng định dạng.");

            if (string.IsNullOrWhiteSpace(user.Phone))
                throw new Exception("Phone không được để trống.");

            string phone = user.Phone.Trim();
            if (!Regex.IsMatch(phone, @"^\d{9,11}$"))
                throw new Exception("Phone phải gồm 9 đến 11 chữ số.");
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}