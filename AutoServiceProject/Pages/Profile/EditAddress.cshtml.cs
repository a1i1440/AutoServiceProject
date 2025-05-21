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
    public class EditAddressModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EditAddressModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            Address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (Address == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);

            var addressInDb = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == Address.Id && a.UserId == userId);
            if (addressInDb == null)
                return NotFound();

            addressInDb.Title = Address.Title;
            addressInDb.FullAddress = Address.FullAddress;

            await _context.SaveChangesAsync();
            return RedirectToPage("MyAddresses");
        }
    }
}
