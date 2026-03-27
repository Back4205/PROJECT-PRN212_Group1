using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LaptopShop.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LaptopShopDbContext _context;

        public OrderRepository()
        {
            _context = new LaptopShopDbContext();
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
                .Include(o => o.Shipment)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public int CountAll()
        {
            return _context.Orders.Count();
        }

        public decimal GetTotalRevenue()
        {
            return _context.Orders
                .Where(o => o.Status == "Completed")
                .Select(o => (decimal?)o.TotalAmount)
                .Sum() ?? 0;
        }

        public int CountByStatus(string status)
        {
            return _context.Orders.Count(o => o.Status == status);
        }
    }
}