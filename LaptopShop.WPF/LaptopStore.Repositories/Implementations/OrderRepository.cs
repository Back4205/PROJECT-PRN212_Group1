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
                _context.Orders.Add(order);
                _context.SaveChanges();

                var groupedItems = items.GroupBy(i => i.ProductId);

                foreach (var group in groupedItems)
                {
                    int productId = group.Key;
                    int quantity = group.Count();

                    // ✅ DEBUG: kiểm tra giá trị thực tế
                    System.Diagnostics.Debug.WriteLine($"ProductId={productId}, Quantity={quantity}");

                    var product = _context.Products
                        .FirstOrDefault(p => p.ProductId == productId);

                    if (product == null)
                        throw new Exception("Product not found.");

                    // ✅ Tắt tracking cache để tránh stale data
                    var productItems = _context.ProductItems
                        .AsNoTracking()
                        .Where(pi => pi.ProductId == productId && pi.Status == "InStock")
                        .Take(quantity)
                        .ToList();

                    // ✅ DEBUG
                    System.Diagnostics.Debug.WriteLine($"Found {productItems.Count} items InStock for ProductId={productId}");

                    if (productItems.Count < quantity)
                        throw new Exception($"Không đủ hàng cho {product.ProductName}");

                    int index = 0;
                    foreach (var item in group)
                    {
                        var pi = _context.ProductItems  // ✅ Load lại với tracking để update
                            .FirstOrDefault(x => x.ProductItemId == productItems[index].ProductItemId);

                        index++;
                        pi.Status = "Sold";

                        item.OrderId = order.OrderId;
                        item.ProductItemId = pi.ProductItemId;
                        item.SnapshotProductName = product.ProductName;
                        item.SnapshotPrice = product.BasePrice;

                        _context.OrderItems.Add(item);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // ✅ DEBUG: xem lỗi thật sự
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");
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
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductItem)  // ✅ Thêm dòng này
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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (order == null) return false;
                if (order.Status != "Pending") return false;

                // ✅ Nhả ProductItem về InStock
                foreach (var item in order.OrderItems)
                {
                    if (item.ProductItemId != null)
                    {
                        var productItem = _context.ProductItems
                            .FirstOrDefault(pi => pi.ProductItemId == item.ProductItemId);

                        if (productItem != null)
                            productItem.Status = "InStock";
                    }
                }

                order.Status = "Cancelled";
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}