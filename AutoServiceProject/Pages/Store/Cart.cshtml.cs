using AutoServiceProject.Data;
using AutoServiceProject.Models;
using AutoServiceProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoServiceProject.Pages.Store
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CartModel(ICartService cartService, AppDbContext context, UserManager<AppUser> userManager)
        {
            _cartService = cartService;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string Address { get; set; }

        public List<CartItem> CartItems => _cartService.GetCartItems();

        public IActionResult OnPostRemove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _cartService.GetCartItems();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Cart is empty.";
                return RedirectToPage();
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                TempData["Error"] = "Address is required.";
                return RedirectToPage();
            }

            foreach (var item in cartItems)
            {
                var order = new Order
                {
                    UserId = userId,
                    SparePartId = item.SparePartId,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.Price,
                    Status = "Pending",
                    OrderDate = DateTime.Now,
                    Address = Address
                };

                _context.Orders.Add(order);
            }

            _cartService.ClearCart();
            await _context.SaveChangesAsync();

            return RedirectToPage("/Profile/Orders");
        }
    }
}
