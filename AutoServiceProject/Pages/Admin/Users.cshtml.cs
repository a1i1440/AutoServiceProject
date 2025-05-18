using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public List<AppUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            var currentUserId = _userManager.GetUserId(User);

            Users = await _userManager.Users
                .Where(u => u.Id != currentUserId)
                .ToListAsync();
        }


        public async Task<IActionResult> OnPostTogglePremiumAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.IsPremium = !user.IsPremium;
            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }
    }
}
