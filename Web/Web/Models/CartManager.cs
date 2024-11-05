using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public static class CartManager
    {
        public static List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Current.Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                HttpContext.Current.Session["Cart"] = cart;
            }
            return cart;
        }

        public static void AddToCart(CartItem item)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(i => i.MaSP == item.MaSP);
            if (existingItem != null)
            {
                existingItem.Soluong += item.Soluong;
            }
            else
            {
                cart.Add(item);
            }
        }

        public static void ClearCart()
        {
            HttpContext.Current.Session["Cart"] = null;
        }
    }
}
