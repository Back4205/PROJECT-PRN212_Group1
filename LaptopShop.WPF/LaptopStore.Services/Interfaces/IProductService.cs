using LaptopShop.Entities.Models;

namespace LaptopShop.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> Search(string keyword);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}