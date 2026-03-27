using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LaptopShop.Repositories.Implementations
{
    public class ProductItemRepository : IProductItemRepository
    {
        private readonly LaptopShopDbContext _context;

        public ProductItemRepository()
        {
            _context = new LaptopShopDbContext();
        }

        public List<ProductItem> GetByWarehouseId(int warehouseId)
        {
            // Sử dụng .Include(pi => pi.Product) để lấy thông tin tên, hãng của Laptop
            return _context.ProductItems
                           .AsNoTracking()
                           .Include(pi => pi.Product)
                           .Where(pi => pi.WarehouseId == warehouseId)
                           .ToList();
        }
        // Trong file Repositories/Implementations/ProductItemRepository.cs
        // Trong ProductItemRepository.cs
        // File: Repositories/Implementations/ProductItemRepository.cs
        public void AddRange(List<ProductItem> items)
        {
            using (var context = new LaptopShopDbContext())
            {
                // 1. Thêm danh sách vào bộ nhớ đệm của EF
                context.ProductItems.AddRange(items);

                // 2. QUAN TRỌNG: Lệnh này mới thực sự đẩy dữ liệu xuống SQL Server
                context.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            var item = _context.ProductItems.Find(id);
            if (item != null)
            {
                _context.ProductItems.Remove(item);
                _context.SaveChanges();
            }
        }
        public void DeleteRange(List<int> ids)
        {
            // Tìm tất cả các item có ID nằm trong danh sách truyền vào
            var itemsToDelete = _context.ProductItems
                                        .Where(pi => ids.Contains(pi.ProductItemId))
                                        .ToList();

            if (itemsToDelete.Any())
            {
                _context.ProductItems.RemoveRange(itemsToDelete);
                _context.SaveChanges();
            }
        }
        public bool IsSerialExists(string serial)
        {
            // Kiểm tra xem số Serial đã tồn tại trong DB chưa (không phân biệt hoa thường)
            return _context.ProductItems.Any(pi => pi.SerialNumber.ToLower() == serial.ToLower());
        }
        public void Update(ProductItem item)
        {
            var existing = _context.ProductItems.Find(item.ProductItemId);
            if (existing != null)
            {
                existing.SerialNumber = item.SerialNumber;
                existing.ProductId = item.ProductId;
                existing.Status = item.Status;

                _context.SaveChanges();

                // Dòng quan trọng: Ép Entity Framework nạp lại thông tin Product mới dựa trên ProductId vừa đổi
                _context.Entry(existing).Reference(p => p.Product).Load();
            }
        }

        public bool IsSerialDuplicate(string serial, int currentId)
        {
            // Tìm xem có máy nào khác (ID khác) mà trùng Serial không
            return _context.ProductItems.Any(pi => pi.SerialNumber.ToLower() == serial.ToLower()
                                                && pi.ProductItemId != currentId);
        }
        public void Add(ProductItem productItem)
        {
            // Thêm đối tượng vào tập hợp ProductItems trong Context
            _context.ProductItems.Add(productItem);

            // Lưu thay đổi xuống SQL Server
            _context.SaveChanges();
        }

    }
}