using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Services.Interfaces
{
    public interface ICartService
    {
        void AddToCart(int userId, int productId, int quantity);
        Cart GetCartByUserId(int userId);
        void RemoveFromCart(int cartItemId);
        void ClearUserCart(int userId);
    }
}
