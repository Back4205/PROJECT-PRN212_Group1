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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
