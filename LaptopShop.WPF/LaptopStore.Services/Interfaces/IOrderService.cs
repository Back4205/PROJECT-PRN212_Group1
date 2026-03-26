using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Interfaces
{
    public interface IOrderService
    {
       
        void PlaceOrder(int userId);
        List<Order> GetOrdersByCustomer(int userId);
        Order GetOrderDetails(int orderId);
        List<Order> GetAll();
        void Update(Order order);
        void Delete(int id);
        bool CancelOrder(int orderId);
    }
}
