using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopShop.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly LaptopShopDbContext context;


        public CartRepository()
        {
            context = new LaptopShopDbContext();
        }

        public void AddItemToCart(int cartId, int productId, int quantity)
        {
            var existingItem = context.CartItems.FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);
            if (existingItem != null)
            {
                // Nếu có rồi thì cộng dồn số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Nếu chưa có thì thêm mới item
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                context.CartItems.Add(newItem);
            }
            context.SaveChanges();
        }

        public void ClearCart(int cartId)
        {
            var items = context.CartItems.Where(ci => ci.CartId == cartId);
            context.CartItems.RemoveRange(items);
            context.SaveChanges();
        }

        public Cart GetCartByCustomerId(int customerId)
        {
            var cart = context.Carts.AsNoTracking()
                 .Include(c => c.CartItems)
                 .ThenInclude(ci => ci.Product) // Load luôn thông tin sản phẩm để hiển thị
                 .FirstOrDefault(c => c.CustomerId == customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.Now
                };
                context.Carts.Add(cart);
                context.SaveChanges();
            }
            return cart;
        }

        public void RemoveItem(int cartItemId)
        {
            var item = context.CartItems.Find(cartItemId);
            if (item != null)
            {
                context.CartItems.Remove(item);
                context.SaveChanges();
            }
        }
        public void UpdateQuantity(int cartItemId, int quantity)
        {
            var item = context.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId);

            if (item != null)
            {
                item.Quantity = quantity;
                context.SaveChanges();
            }
        }
        public int GetStockCount(int productId)
        {
            return context.ProductItems
                .Count(pi => pi.ProductId == productId && pi.Status == "InStock");
        }
    }
}
