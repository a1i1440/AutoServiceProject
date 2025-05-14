using System.Text.Json;
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

        public List<string> CarBrands { get; set; }
        public List<string> Services { get; set; } = new List<string>
            {
                "Painting",
                "Part Replacement",
                "Oil Change",
                "Brake Check",
                "Tire Change",
                "Battery Inspection",
                "Engine Diagnostics",
                "General Maintenance",
                "Other"
            };

        public List<Mechanic> FilteredMechanics { get; set; }

        public async Task OnGetAsync()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/car-brands.json");
            var json = System.IO.File.ReadAllText(jsonPath);

            CarBrands = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

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
