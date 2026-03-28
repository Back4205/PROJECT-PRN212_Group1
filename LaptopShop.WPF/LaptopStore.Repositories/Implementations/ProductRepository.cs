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
            // 1. Tìm sản phẩm trong bảng Product
            var product = _context.Products.Find(id);

            if (product != null)
            {
                // 2. Kiểm tra xem sản phẩm này đã từng xuất hiện trong đơn hàng nào chưa
                bool hasBeenOrdered = _context.OrderItems.Any(oi => oi.ProductId == id);

                if (hasBeenOrdered)
                {
                    // CASE 1: Đã có trong đơn hàng -> Chỉ ẩn đi (Bỏ tích Active)
                    product.IsActive = false;
                    _context.SaveChanges();
                    // Má có thể quăng một cái message nhẹ ở đây: "Sản phẩm đã có đơn hàng nên chỉ chuyển sang trạng thái Ngưng bán."
                }
                else
                {
                    // CASE 2: Chưa có đơn hàng nào -> Xóa sổ luôn
                    _context.Products.Remove(product);
                    _context.SaveChanges();
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

            // 1. Tìm theo tên HOẶC mã sản phẩm (Thêm tìm theo mã để search LAP001 được)
           if (!string.IsNullOrEmpty(keyword)) if (!string.IsNullOrEmpty(keyword))
                {
                  
                    products = products.Where(p => p.ProductName.Contains(keyword)
                                                || p.ProductCode.Contains(keyword)
                                                || p.Brand.Contains(keyword));
                }

            // 2. Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand == brand);
            }

            // 3. FIX LỖI OVERFLOW: Khống chế maxPrice không được quá lớn so với SQL
            decimal sqlMaxDecimal = 999999999999999; // Giới hạn an toàn cho SQL
            if (maxPrice > sqlMaxDecimal) maxPrice = sqlMaxDecimal;
            if (minPrice < 0) minPrice = 0;

            products = products.Where(p => p.BasePrice >= minPrice && p.BasePrice <= maxPrice);

            return products.ToList();
        }


        public void Update(Product product)
        {
            var oldProduct = _context.Products.Find(product.ProductId);
            if (oldProduct != null)
            {
                oldProduct.ProductName = product.ProductName;
                oldProduct.ProductCode = product.ProductCode;
                oldProduct.Brand = product.Brand;
                oldProduct.BasePrice = product.BasePrice;
                oldProduct.ImgUrl = product.ImgUrl;

                // PHẢI CÓ DÒNG NÀY THÌ NÓ MỚI TÍCH LẠI ĐƯỢC NÈ MÁ:
                oldProduct.IsActive = product.IsActive;

                _context.SaveChanges();
            }
        }
    }
}
