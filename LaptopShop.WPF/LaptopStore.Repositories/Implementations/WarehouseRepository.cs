using LaptopShop.Entities.Models;
using LaptopShop.Repositories;

public class WarehouseRepository
{
    private readonly LaptopShopDbContext _context;
    public WarehouseRepository() => _context = new LaptopShopDbContext();

    public List<Warehouse> GetAll()
    {
        return _context.Warehouses.ToList();
    }
}