using LaptopShop.Entities.Models;

namespace LaptopShop.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product GetByCode(string productCode);
        List<Product> Search(string keyword);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}