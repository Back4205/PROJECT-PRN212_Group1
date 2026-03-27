using LaptopShop.Entities.Models;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> origin/Qui

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
<<<<<<< HEAD
       
        

       
       
        public List<Product> SearchAndFilter(string keyword,string brand ,  decimal minPrice, decimal maxPrice);
    }
}
=======
    }
}
>>>>>>> origin/Qui
