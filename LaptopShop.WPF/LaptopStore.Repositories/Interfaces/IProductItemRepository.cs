using LaptopShop.Entities.Models;
using System.Collections.Generic;

namespace LaptopShop.Repositories.Interfaces
{
    public interface IProductItemRepository
    {
        bool IsSerialExists(string serial);
        void AddRange(List<ProductItem> newItems);
        List<ProductItem> GetByWarehouseId(int warehouseId);

        // Thêm dòng này để định nghĩa hàm xóa
        void Delete(int id);
        void DeleteRange(List<int> ids);
        void Update(ProductItem item);
        void Add(ProductItem productItem);
        bool IsSerialDuplicate(string newSerial, int currentItemId);
    }
}