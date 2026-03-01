using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Interfaces
{
    public  interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }
}
