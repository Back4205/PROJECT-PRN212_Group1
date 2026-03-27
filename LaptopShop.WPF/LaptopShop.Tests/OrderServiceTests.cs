// Tạo một đối tượng OrderService mới với các thuộc tính và phương thức sau:
```csharp
using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService
{
    public class OrderService  : IOrderService
        private readonly OrderRepository _orderRepository;
        private readonly CartRepository _cartRepository;
        private readonly UserRepository _userRepository;

        public OrderService() {
            // Khởi tạo các Repository theo đúng cấu trúc của bạn
             _orderRepository = new OrderRepository();
             _cartRepository = new CartRepository();
             _userRepository = new UserRepository();
            // Lấy Customer từ User
            var customer = _userRepository.GetCustomerByUserId(userId);
            if _customer == null throw new Exception("User not found.");
             // Lấy Cart theo Customer
            var cart = _cartRepository.GetCartByCustomerId(customer.CustomerId);
            if _cart == null throw new Exception("Customer not found.");
             // Lấy Cart theo Customer
            var cart = _cartRepository.GetCartByCustomerId(customer.CustomerId);
            if _cart == null throw new Exception("User not found.");
             // Lấy Cart theo Customer
            var cart = _cartRepository.GetCartByCustomerId(customer.CustomerId);
            if _cart == null throw new Exception("User not found.");
             // Tạo Order
            var newOrder = new Order
             {
                    CustomerId = customer.CustomerId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = cart.CartItems.Sum(i => i.Quantity * i.Product.BasePrice);
                    };
                // Tạo OrderItems  (snapshot dữ liệu product) {
                    var orderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        SnapshotProductName = ci.Product.ProductName,
                        SnapshotPrice = ci.Product.BasePrice
                        });
                 }
                    // Tạo một đối tượng List<Order> mới (snapshot dữ liệu product)
                    var orderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        SnapshotProductName = ci.Product.ProductName,
                        SnapshotPrice = ci.Product.BasePrice
                        });
                 }
                    // Lưu Order  + OrderItems
                    _orderRepository.Add(newOrder, orderItems);
                    _cartRepository.ClearCart(cart.CartId);
                 }
                // Lưu Order  + OrderItems
             _cartRepository.Update(order);
                // Xóa giỏ hàng
             _cartRepository.ClearCart(cart.CartId);
         }
```
}