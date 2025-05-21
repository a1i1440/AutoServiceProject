using AutoServiceProject.Data;
using AutoServiceProject.Models;
using AutoServiceProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public List<CartItem> CartItems => _cartService.GetCartItems();

        public List<Address> SavedAddresses { get; set; } = new();

        [BindProperty]
        public string SelectedAddressId { get; set; }

        [BindProperty]
        public string CustomAddress { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            SavedAddresses = await _context.Addresses
                .Where(a => a.UserId == user.Id)
                .ToListAsync();
        }

        public IActionResult OnPostRemove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItems = _cartService.GetCartItems();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Cart is empty.";
                return RedirectToPage();
            }

            string finalAddress;

            if (!string.IsNullOrWhiteSpace(CustomAddress))
            {
                finalAddress = CustomAddress;
            }
            else if (!string.IsNullOrWhiteSpace(SelectedAddressId) && int.TryParse(SelectedAddressId, out var addrId))
            {
                var selected = await _context.Addresses.FindAsync(addrId);
                finalAddress = selected?.FullAddress ?? "Unknown Address";
            }
            else
            {
                TempData["Error"] = "Please select or write an address.";
                await OnGetAsync();
                return Page();
            }

            foreach (var item in cartItems)
            {
                var order = new Order
                {
                    UserId = user.Id,
                    SparePartId = item.SparePartId,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.Price,
                    Status = "Pending",
                    OrderDate = DateTime.Now,
                    Address = finalAddress
                };

                _context.Orders.Add(order);
            }

            _cartService.ClearCart();
            await _context.SaveChangesAsync();

            return RedirectToPage("/Profile/Orders");
        }
    }
}
