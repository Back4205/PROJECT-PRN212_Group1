using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Implementations
{
    public  class ProductRepository : IProductRepository 
=======
using Microsoft.EntityFrameworkCore;

namespace LaptopShop.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
>>>>>>> origin/Qui
    {
        private readonly LaptopShopDbContext _context;

        public ProductRepository()
        {
            _context = new LaptopShopDbContext();
        }

<<<<<<< HEAD
        public void Add(Product product)
        {
            _context.Add(product);
=======
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
>>>>>>> origin/Qui
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
<<<<<<< HEAD
            if(product != null)
=======
            if (product != null)
>>>>>>> origin/Qui
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
<<<<<<< HEAD

       

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public List<Product> SearchAndFilter(string keyword, string brand, decimal minPrice, decimal maxPrice)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.ProductName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand == brand);
            }

            products = products.Where(p => p.BasePrice >= minPrice && p.BasePrice <= maxPrice);

            return products.ToList();
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
=======
    }
}
>>>>>>> origin/Qui
