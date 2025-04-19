using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Service
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CreateModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ServiceRequest ServiceRequest { get; set; }

        public List<string> Brands { get; set; } = new List<string> { "Toyota", "BMW", "Mercedes" };
        public List<string> Services { get; set; } = new List<string> { "Boyama", "Parça Deyiþimi", "Diger" };

        public List<Mechanic> FilteredMechanics { get; set; }

        public async Task OnGetAsync()
        {
            FilteredMechanics = await _context.Mechanics.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            ServiceRequest.UserId = user.Id;

            _context.ServiceRequests.Add(ServiceRequest);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Profile/MyServices"); 
        }
    }
}
