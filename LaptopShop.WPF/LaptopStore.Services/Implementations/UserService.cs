using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Implementations
{
    public  class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        public UserService() { 
        _userRepository = new UserRepository();
        }

        public User Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}
