using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaptopShop.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LaptopShopDbContext _context;

        public OrderRepository()
        {
            _context = new LaptopShopDbContext();
        }

        public void Add(Order order, List<OrderItem> items)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1. Lưu Order trước để lấy OrderID
                _context.Orders.Add(order);
                _context.SaveChanges();

                // 2. Thêm OrderItems
                foreach (var item in items)
                {
                    item.OrderId = order.OrderId;

                    var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (product == null)
                        throw new Exception("Product not found.");

                    // Snapshot thông tin product
                    item.SnapshotProductName = product.ProductName;
                    item.SnapshotPrice = product.BasePrice;

                    _context.OrderItems.Add(item);
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public List<Order> GetAllProductByCustomerID(int customerId)
        {
            return _context.Orders
           .Where(o => o.CustomerId == customerId)
           .Include(o => o.OrderItems)
           .ThenInclude(oi => oi.Product)
           .OrderByDescending(o => o.OrderDate)
           .ToList();
        }

        public void Update(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
            if (existingOrder == null) return;

            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            _context.SaveChanges();
        }

        public void UpdateStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return;

            order.Status = status;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return;

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public bool CancelOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                return false;

            if (order.Status != "Pending")
                return false;

            order.Status = "Cancelled";
            _context.SaveChanges();

            return true;
        }
    }
}