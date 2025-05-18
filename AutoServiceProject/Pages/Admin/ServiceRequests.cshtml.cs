using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ServiceRequestsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ServiceRequestsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<ServiceRequest> Requests { get; set; }

        public async Task OnGetAsync()
        {
            Requests = await _context.ServiceRequests
                .Include(r => r.MechanicUser)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}
