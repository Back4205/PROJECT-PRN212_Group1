using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using LaptopShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly CartRepository _repository;
        private readonly UserRepository _userRepository;

        public CartService(CartRepository repository, UserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            var customer = _userRepository.GetCustomerByUserId(userId);
            if (customer == null) throw new Exception("Customer not found!");

            var cart = _repository.GetCartByCustomerId(customer.CustomerId);

            _repository.AddItemToCart(cart.CartId, productId, quantity);
        }

        public void ClearUserCart(int userId)
        {
            var customer = _userRepository.GetCustomerByUserId(userId);
            if (customer == null) throw new Exception("Customer not found!");

            var cart = _repository.GetCartByCustomerId(customer.CustomerId);
            _repository.ClearCart(cart.CartId);
        }

        public Cart GetCartByUserId(int userId)
        {
            var customer = _userRepository.GetCustomerByUserId(userId);
            if (customer == null) throw new Exception("Customer not found!");

            return _repository.GetCartByCustomerId(customer.CustomerId);
        }

        public void RemoveFromCart(int cartItemId)
        {
            _repository.RemoveItem(cartItemId);
        }
    }
}
