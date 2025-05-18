using System.Text.Json;
using AutoServiceProject.Data;
using AutoServiceProject.Hubs;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MechanicModel = AutoServiceProject.Models.Mechanic;

namespace AutoServiceProject.Pages.Service
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private readonly IHubContext<ServiceRequestHub> _hubContext;

        public CreateModel(AppDbContext context, UserManager<AppUser> userManager, IHubContext<ServiceRequestHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
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

        public List<MechanicModel> FilteredMechanics { get; set; } = new List<MechanicModel>();


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
            ServiceRequest.RequestDate = DateTime.Now;
            _context.ServiceRequests.Add(ServiceRequest);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("NewServiceRequest");
            return RedirectToPage("/Profile/MyServices");
        }

        public JsonResult OnGetMechanicsByBrand(string brand)
        {
            var mechanics = _context.Mechanics
                .Where(m => m.SpecializationBrand == brand)
                .Select(m => new
                {
                    id = m.Id,
                    name = $"{m.FullName} – {m.SpecializationBrand}"
                })
                .ToList();

            return new JsonResult(mechanics);
        }
    }
}
