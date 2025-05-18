using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Profile
{
    public class MyServicesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MyServicesModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ServiceRequest> Requests { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            Requests = await _context.ServiceRequests
                .Include(r => r.Mechanic)
                 .Include(r => r.MechanicUser)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }
    }
}
