using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public void Add(Product product)
        {
            _productRepository.Add(product);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        

        public List<Product> GetAll()
        {
           return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return _productRepository.GetAll();
            }else
            {
                return _productRepository.SearchAndFilter(keyword, null, 0, decimal.MaxValue);
            }
               
            
        }

        public List<Product> SearchAndFilter(string keyword, string brand, decimal minPrice, decimal maxPrice)
        {
            return _productRepository.SearchAndFilter(keyword, brand, minPrice, maxPrice);
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
