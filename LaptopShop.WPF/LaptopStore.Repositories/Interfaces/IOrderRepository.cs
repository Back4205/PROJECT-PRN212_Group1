using LaptopShop.Entities.Models;

namespace LaptopShop.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);

        int CountAll();
        decimal GetTotalRevenue();
        int CountByStatus(string status);
    }
}