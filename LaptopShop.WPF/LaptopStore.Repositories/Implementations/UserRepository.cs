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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
