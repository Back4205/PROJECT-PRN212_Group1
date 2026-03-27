using LaptopShop.Entities.Models;
<<<<<<< HEAD
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
        void Add(Order order, List<OrderItem> Items);
        void Update(Order order);
        void Delete(int id);
        void UpdateStatus(int orderId, string status);
        List<Order> GetAllProductByCustomerID(int id);
        bool CancelOrder(int orderId);
    }
}
=======

namespace LaptopShop.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);
        List<OrderItem> GetOrderItemsByOrderId(int orderId);

        void Add(Order order);
        void Update(Order order);
        void Delete(int id);

        int CountAll();
        decimal GetTotalRevenue();
        int CountByStatus(string status);
    }
}
>>>>>>> origin/Qui
