using LaptopShop.Entities.Models;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> origin/Qui

namespace LaptopShop.Services.Interfaces
{
    public interface IOrderService
    {
<<<<<<< HEAD
       
        void PlaceOrder(int userId);
        List<Order> GetOrdersByCustomer(int userId);
        Order GetOrderDetails(int orderId);
        List<Order> GetAll();
        void Update(Order order);
        void Delete(int id);
        bool CancelOrder(int orderId);
    }
}
=======
        List<Order> GetAll();
        Order GetById(int id);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);

        void CreateOrder(Order order);
        void Update(Order order);
        void Delete(int id);

        int GetTotalOrders();
        decimal GetTotalRevenue();
        int GetOrderCountByStatus(string status);
    }
}
>>>>>>> origin/Qui
