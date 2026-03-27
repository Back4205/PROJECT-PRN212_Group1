using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace LaptopShop.Services.Implementations
{
    public  class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        public UserService() { 
        _userRepository = new UserRepository();
        }

        public List<Role> GetRolesByUserId(int userId)
        {
            return _userRepository.GetRolesByUserId(userId) ?? new List<Role>();
        }

        public bool IsEmailExists(string email)
        {
            return _userRepository.GetAll()
                .Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool IsUsernameExists(string username)
        {
            return _userRepository.GetAll()
                .Any(u => u.Username.ToLower() == username.ToLower());
        }

        public User Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null)
                return null;

            if (!BC.Verify(password, user.PasswordHash))
                return null;

            
            user.Roles = _userRepository.GetRolesByUserId(user.UserId);

            return user;
        }

        public void Register(User user)
        {
            if (!user.Email.EndsWith("@gmail.com"))
                throw new Exception("Email phải đúng định dạng ");

            if (user.PasswordHash.Length <= 8)
                throw new Exception("Mật khẩu phải nhiều hơn hoặc bằng 8 ký tự ");

            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            _userRepository.Add(user);
            _userRepository.AddUserRole(user.UserId, 4);
        }

        public Customer GetCustomerByUserId(int userId)
        {

            return _userRepository.GetCustomerByUserId(userId);
        }


        // Trong UserService.cs (IUserService cũng cần thêm các khai báo tương ứng)
        public User GetUserById(int id) => _userRepository.GetById(id);

        public void UpdateUserProfile(User user, string newPassword, string address = null)
        {
            // 1. Cập nhật mật khẩu nếu người dùng nhập mới
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword.Length < 8) throw new Exception("Mật khẩu mới phải từ 8 ký tự.");
                user.PasswordHash = BC.HashPassword(newPassword);
            }

            // 2. Cập nhật thông tin User
            _userRepository.Update(user);

            // 3. Nếu có địa chỉ (là Customer), cập nhật bảng Customer
            if (address != null)
            {
                var customer = _userRepository.GetCustomerByUserId(user.UserId);
                if (customer != null)
                {
                    customer.Address = address;
                    _userRepository.UpdateCustomer(customer);
                }
            }
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
                throw new Exception("Người dùng phải có ít nhất 1 role.");

            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new Exception("Không tìm thấy người dùng.");

            var currentRoles = _userRepository.GetRolesByUserId(userId);
            bool wasAdmin = currentRoles.Any(r => r.RoleName == "Admin");

            var allRoles = _userRepository.GetAllRoles();
            var selectedRoles = allRoles.Where(r => roleIds.Contains(r.RoleId)).ToList();
            bool willBeAdmin = selectedRoles.Any(r => r.RoleName == "Admin");

            // Đếm số admin active hiện tại
            int activeAdminCount = _userRepository.GetAll()
                .Count(u => u.IsActive && _userRepository.GetRolesByUserId(u.UserId).Any(r => r.RoleName == "Admin"));

            if (user.IsActive && wasAdmin && !willBeAdmin && activeAdminCount <= 1)
                throw new Exception("Không thể bỏ role Admin của admin cuối cùng.");

            _userRepository.UpdateUserRoles(userId, roleIds);
        }

        public void SetUserActiveStatus(int userId, bool isActive)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new Exception("Không tìm thấy người dùng.");

            var roles = _userRepository.GetRolesByUserId(userId);
            bool isAdmin = roles.Any(r => r.RoleName == "Admin");

            int activeAdminCount = _userRepository.GetAll()
                .Count(u => u.IsActive && _userRepository.GetRolesByUserId(u.UserId).Any(r => r.RoleName == "Admin"));

            if (!isActive && user.IsActive && isAdmin && activeAdminCount <= 1)
                throw new Exception("Không thể khóa admin cuối cùng.");

            user.IsActive = isActive;
            _userRepository.Update(user);
        }

        public void AddUserByAdmin(User user, List<int> roleIds)
        {
            if (user == null)
                throw new Exception("Dữ liệu user không hợp lệ.");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username không được để trống.");

            if (user.Username.Trim().Length < 3)
                throw new Exception("Username phải có ít nhất 3 ký tự.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new Exception("Password không được để trống.");

            if (user.PasswordHash.Trim().Length < 8)
                throw new Exception("Password phải có ít nhất 8 ký tự.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Full name không được để trống.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email không được để trống.");

            if (!Regex.IsMatch(user.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Email không đúng định dạng.");

            if (string.IsNullOrWhiteSpace(user.Phone))
                throw new Exception("Phone không được để trống.");

            if (!Regex.IsMatch(user.Phone.Trim(), @"^\d{9,11}$"))
                throw new Exception("Phone phải gồm 9 đến 11 chữ số.");

            if (roleIds == null || roleIds.Count == 0)
                throw new Exception("User phải có ít nhất 1 role.");

            if (IsUsernameExists(user.Username))
                throw new Exception("Username đã tồn tại.");

            if (IsEmailExists(user.Email))
                throw new Exception("Email đã tồn tại.");

            user.Username = user.Username.Trim();
            user.FullName = user.FullName.Trim();
            user.Email = user.Email.Trim();
            user.Phone = user.Phone.Trim();
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.IsActive = true;

            _userRepository.Add(user);
            _userRepository.UpdateUserRoles(user.UserId, roleIds);
        }

    }
}
