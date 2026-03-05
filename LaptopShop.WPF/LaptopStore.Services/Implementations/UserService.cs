using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
