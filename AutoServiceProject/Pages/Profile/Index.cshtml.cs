using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoServiceProject.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public AppUser CurrentUser { get; set; }

        [BindProperty]
        public IFormFile ProfilePhoto { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (ProfilePhoto != null)
            {
                var uploadsFolder = Path.Combine("wwwroot/images/profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ProfilePhoto.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }

                CurrentUser.ProfileImageUrl = "/images/profiles/" + fileName;
                await _userManager.UpdateAsync(CurrentUser);
            }

            return RedirectToPage();
        }
    }
}
