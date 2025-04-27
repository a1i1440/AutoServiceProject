using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public EditModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string FullName { get; set; }
            public string Address { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Input = new InputModel
            {
                FullName = user.FullName,
                Address = user.Address
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            user.FullName = Input.FullName;
            user.Address = Input.Address;

            await _userManager.UpdateAsync(user);

            return RedirectToPage("./Index");
        }
    }
}
