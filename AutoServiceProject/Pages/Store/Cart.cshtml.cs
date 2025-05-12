using AutoServiceProject.Models;
using AutoServiceProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoServiceProject.Pages.Store
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;

        public CartModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public List<CartItem> CartItems => _cartService.GetCartItems();

        public IActionResult OnPostRemove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            _cartService.ClearCart();
            return RedirectToPage("/Profile/Orders");
        }
    }
}
