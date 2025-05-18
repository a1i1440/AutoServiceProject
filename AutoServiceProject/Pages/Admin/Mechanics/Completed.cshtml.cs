using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Mechanic
{
    [Authorize(Roles = "Mechanic")]
    public class CompletedModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CompletedModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ServiceRequest> CompletedRequests { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            CompletedRequests = await _context.ServiceRequests
                 .Include(r => r.User)
                .Where(r => r.MechanicUserId == userId && r.Status == "Completed")
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }
    }
}
