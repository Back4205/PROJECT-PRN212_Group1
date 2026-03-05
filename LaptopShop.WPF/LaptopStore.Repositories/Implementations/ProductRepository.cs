using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Implementations
{
    public  class ProductRepository : IProductRepository 
    {
        private readonly LaptopShopDbContext _context;

        public ProductRepository()
        {
            _context = new LaptopShopDbContext();
        }

        public void Add(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if(product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<Product> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            return _context.Products
                   .Where(p => p.BasePrice >= minPrice && p.BasePrice <= maxPrice)
                   .ToList();
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public List<Product> SearchByName(string keyword)
        {
            return _context.Products.Where(p => p.ProductName.Contains(keyword)).ToList();
        }

        public void Update(Product product)
        {
            var oldProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (oldProduct != null)
            {
                oldProduct.ProductName = product.ProductName;
                oldProduct.ProductCode = product.ProductCode;
                oldProduct.Brand = product.Brand;
                oldProduct.BasePrice = product.BasePrice;
                oldProduct.ImgUrl = product.ImgUrl;

                _context.SaveChanges();
            }
        }
    }
}
