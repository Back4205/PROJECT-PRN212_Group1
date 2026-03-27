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
            var item = _context.ProductItems.Find(id);
            if (item != null)
            {
                // Chỉ cho phép xóa nếu trạng thái là InStock hoặc Defective
                if (item.Status == "InStock" || item.Status == "Defective")
                {
                    _context.ProductItems.Remove(item);
                    _context.SaveChanges();
                }
                else
                {
                    // Quăng lỗi để tầng giao diện (UI) có thể bắt và hiển thị thông báo
                    throw new InvalidOperationException($"Không thể xóa thiết bị đang ở trạng thái: {item.Status}");
                }
            }
        }



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
