using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

=======
>>>>>>> origin/Qui
namespace LaptopShop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

<<<<<<< HEAD
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
=======
        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
>>>>>>> origin/Qui
        }

        public Product GetById(int id)
        {
<<<<<<< HEAD
            throw new NotImplementedException();
=======
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                throw new Exception("Không tìm thấy sản phẩm.");
            }

            return product;
>>>>>>> origin/Qui
        }

        public List<Product> Search(string keyword)
        {
<<<<<<< HEAD
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
=======
            return _productRepository.Search(keyword);
        }

        public void Add(Product product)
        {
            ValidateProduct(product);

            var existing = _productRepository.GetByCode(product.ProductCode.Trim());
            if (existing != null)
            {
                throw new Exception("Mã sản phẩm đã tồn tại.");
            }

            product.ProductCode = product.ProductCode.Trim();
            product.ProductName = product.ProductName.Trim();
            product.Brand = product.Brand.Trim();
            product.ImgUrl = string.IsNullOrWhiteSpace(product.ImgUrl) ? null : product.ImgUrl.Trim();
            product.IsActive = true;

            _productRepository.Add(product);
>>>>>>> origin/Qui
        }

        public void Update(Product product)
        {
<<<<<<< HEAD
            throw new NotImplementedException();
        }
    }
}
=======
            ValidateProduct(product);

            var existing = _productRepository.GetById(product.ProductId);
            if (existing == null)
            {
                throw new Exception("Không tìm thấy sản phẩm.");
            }

            var duplicateCode = _productRepository.GetByCode(product.ProductCode.Trim());
            if (duplicateCode != null && duplicateCode.ProductId != product.ProductId)
            {
                throw new Exception("Mã sản phẩm đã tồn tại.");
            }

            existing.ProductCode = product.ProductCode.Trim();
            existing.ProductName = product.ProductName.Trim();
            existing.Brand = product.Brand.Trim();
            existing.BasePrice = product.BasePrice;
            existing.ImgUrl = string.IsNullOrWhiteSpace(product.ImgUrl) ? null : product.ImgUrl.Trim();
            existing.IsActive = product.IsActive;

            _productRepository.Update(existing);
        }

        public void Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                throw new Exception("Không tìm thấy sản phẩm.");
            }

            if (product.OrderItems != null && product.OrderItems.Any())
            {
                product.IsActive = false;
                _productRepository.Update(product);
                return;
            }

            _productRepository.Delete(id);
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new Exception("Dữ liệu sản phẩm không hợp lệ.");

            if (string.IsNullOrWhiteSpace(product.ProductCode))
                throw new Exception("Mã sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new Exception("Tên sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(product.Brand))
                throw new Exception("Brand không được để trống.");

            if (product.BasePrice <= 0)
                throw new Exception("Giá phải lớn hơn 0.");
        }
    }
}
>>>>>>> origin/Qui
