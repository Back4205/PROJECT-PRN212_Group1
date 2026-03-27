using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;

namespace LaptopShop.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService()
        {
            _orderRepository = new OrderRepository();
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy đơn hàng.");
            }
            return order;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _orderRepository.GetOrderItemsByOrderId(orderId);
        }

        public void CreateOrder(Order order)
        {
            if (order == null)
                throw new Exception("Order không hợp lệ.");

            order.OrderDate = DateTime.Now;
            if (string.IsNullOrEmpty(order.Status))
                order.Status = "Pending";

            _orderRepository.Add(order);
        }

        public void Update(Order order)
        {
            if (order == null)
                throw new Exception("Order không hợp lệ.");

            _orderRepository.Update(order);
        }

        public void Delete(int id)
        {
            _orderRepository.Delete(id);
        }

        public int GetTotalOrders()
        {
            return _orderRepository.CountAll();
        }

        public decimal GetTotalRevenue()
        {
            return _orderRepository.GetTotalRevenue();
        }

        public int GetOrderCountByStatus(string status)
        {
            return _orderRepository.CountByStatus(status);
        }
    }
}