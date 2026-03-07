using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       
        

       
       
        public List<Product> SearchAndFilter(string keyword,string brand ,  decimal minPrice, decimal maxPrice);
    }
}
