using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly LaptopShopDbContext _context;

        public ProductRepository()
        {
            _context = new LaptopShopDbContext();
        }

        public List<Product> GetAll()
        {
            return _context.Products
                .OrderByDescending(p => p.ProductId)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.OrderItems)
                .FirstOrDefault(p => p.ProductId == id);
        }

        public Product GetByCode(string productCode)
        {
            return _context.Products
                .FirstOrDefault(p => p.ProductCode == productCode);
        }

        public List<Product> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAll();
            }

            keyword = keyword.Trim().ToLower();

            return _context.Products
                .Where(p =>
                    p.ProductCode.ToLower().Contains(keyword) ||
                    p.ProductName.ToLower().Contains(keyword) ||
                    p.Brand.ToLower().Contains(keyword))
                .OrderByDescending(p => p.ProductId)
                .ToList();
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}