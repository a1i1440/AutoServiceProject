using AutoServiceProject.Models;
using System.Collections.Generic;

namespace AutoServiceProject.Services
{
    public interface ICartService
    {
        void AddToCart(CartItem item);
        void RemoveFromCart(int sparePartId);
        void ClearCart();
        List<CartItem> GetCartItems(); 
    }
}
