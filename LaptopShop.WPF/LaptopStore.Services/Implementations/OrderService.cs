using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public void CreateOrder(Order order)
        {
            if (order == null)
            {
                throw new Exception("Dữ liệu đơn hàng không hợp lệ.");
            }

            if (order.CustomerId <= 0)
            {
                throw new Exception("Customer không hợp lệ.");
            }

            if (order.TotalAmount < 0)
            {
                throw new Exception("Tổng tiền không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(order.Status))
            {
                order.Status = "Pending";
            }

            order.OrderDate = DateTime.Now;
            _orderRepository.Add(order);
        }

        public void Update(Order order)
        {
            if (order == null)
            {
                throw new Exception("Dữ liệu đơn hàng không hợp lệ.");
            }

            var existing = _orderRepository.GetById(order.OrderId);
            if (existing == null)
            {
                throw new Exception("Không tìm thấy đơn hàng.");
            }

            existing.Status = order.Status;
            existing.TotalAmount = order.TotalAmount;

            _orderRepository.Update(existing);
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