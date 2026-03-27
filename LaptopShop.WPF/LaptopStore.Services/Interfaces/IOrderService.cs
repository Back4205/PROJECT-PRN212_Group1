using LaptopShop.Entities.Models;

namespace LaptopShop.Services.Interfaces
{
    public interface IOrderService
    {
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