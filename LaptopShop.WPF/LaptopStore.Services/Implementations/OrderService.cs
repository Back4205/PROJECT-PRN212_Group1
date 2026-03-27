using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
=======
>>>>>>> origin/Qui

namespace LaptopShop.Services.Implementations
{
    public class OrderService : IOrderService
    {
<<<<<<< HEAD
        private readonly OrderRepository _orderRepository;
        private readonly CartRepository _cartRepository;
        private readonly UserRepository _userRepository;

        public OrderService()
        {
            // Khởi tạo các Repository theo đúng cấu trúc của bạn
            _orderRepository = new OrderRepository();
            _cartRepository = new CartRepository();
            _userRepository = new UserRepository();
        }

        public void PlaceOrder(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Lấy Customer từ User
            var customer = _userRepository.GetCustomerByUserId(userId);
            if (customer == null)
                throw new Exception("Customer not found.");

            // Lấy Cart theo Customer
            var cart = _cartRepository.GetCartByCustomerId(customer.CustomerId);
            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Giỏ hàng của bạn đang trống!");

            // Tạo Order
            var newOrder = new Order
            {
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = cart.CartItems.Sum(i => i.Quantity * i.Product.BasePrice)
            };

            // Tạo OrderItems (snapshot dữ liệu product)
            //var orderItems = cart.CartItems.Select(ci => new OrderItem
            //{
            //    ProductId = ci.ProductId,
            //    SnapshotProductName = ci.Product.ProductName,
            //    SnapshotPrice = ci.Product.BasePrice
            //}).ToList();
            var orderItems = new List<OrderItem>();

            foreach (var ci in cart.CartItems)
            {
                // Nếu Quantity là 5, vòng lặp này sẽ chạy 5 lần để tạo 5 bản ghi
                for (int i = 0; i < ci.Quantity; i++)
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = ci.ProductId,
                        SnapshotProductName = ci.Product.ProductName,
                        SnapshotPrice = ci.Product.BasePrice
                        // Chú ý: Quantity trong bảng OrderItem của bạn không có, 
                        // vì mỗi dòng này đại diện cho 1 máy duy nhất.
                    });
                }
            }



            // Lưu Order + OrderItems
            _orderRepository.Add(newOrder, orderItems);

            // Xóa giỏ hàng
            _cartRepository.ClearCart(cart.CartId);
        }

        public List<Order> GetOrdersByCustomer(int userId)
        {
            var customer = _userRepository.GetCustomerByUserId(userId);
            if (customer == null) throw new Exception("Customer not found.");

            return _orderRepository.GetAllProductByCustomerID(customer.CustomerId);
        }

        public Order GetOrderDetails(int orderId)
        {
            return _orderRepository.GetById(orderId);
=======
        private readonly IOrderRepository _orderRepository;

        public OrderService()
        {
            _orderRepository = new OrderRepository();
>>>>>>> origin/Qui
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

<<<<<<< HEAD
        public void Update(Order order)
        {
=======
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

>>>>>>> origin/Qui
            _orderRepository.Update(order);
        }

        public void Delete(int id)
        {
            _orderRepository.Delete(id);
        }
<<<<<<< HEAD
        public bool CancelOrder(int orderId)
        {
           return _orderRepository.CancelOrder(orderId);
=======

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
>>>>>>> origin/Qui
        }
    }
}