using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Profile
{
    [Authorize]
    public class MyAddressesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MyAddressesModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Address> SavedAddresses { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            SavedAddresses = await _context.Addresses
                .Where(a => a.UserId == user.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var title = Request.Form["Title"];
            var fullAddress = Request.Form["FullAddress"];

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(fullAddress))
                return RedirectToPage();

            var user = await _userManager.GetUserAsync(User);

            var newAddress = new Address
            {
                Title = title,
                FullAddress = fullAddress,
                UserId = user.Id
            };

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
