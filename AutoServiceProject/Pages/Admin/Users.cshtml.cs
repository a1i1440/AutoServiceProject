using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var allUsers = _userManager.Users.ToList();
            var currentUser = await _userManager.GetUserAsync(User);

            Users = allUsers
                .Where(u => u.Id != currentUser.Id) 
                .ToList();
        }

    }
}
