using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Interfaces
{
    public  interface IUserService
    {
        User Login(string username, string password);
        void Register(User user);
    }
}
