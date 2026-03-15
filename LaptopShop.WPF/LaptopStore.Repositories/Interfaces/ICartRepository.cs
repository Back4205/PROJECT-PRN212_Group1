using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Cart GetCartByCustomerId(int customerId);
        void AddItemToCart(int cartId, int productId, int quantity);
        void RemoveItem(int cartItemId);
        void ClearCart(int cartId);
    }
}
