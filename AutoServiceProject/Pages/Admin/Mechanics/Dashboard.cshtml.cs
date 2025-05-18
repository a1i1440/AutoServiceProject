using AutoServiceProject.Data;
using AutoServiceProject.Hubs;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Mechanic
{
    [Authorize(Roles = "Mechanic")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ServiceRequestHub> _hubContext;
        public DashboardModel(AppDbContext context, UserManager<AppUser> userManager, IHubContext<ServiceRequestHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public List<ServiceRequest> MyRequests { get; set; }
        public string CurrentUserId { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUserId = _userManager.GetUserId(User);

            MyRequests = await _context.ServiceRequests
                .Where(r =>
                    r.Status == "Pending" ||
                    (r.MechanicUserId == CurrentUserId && r.Status != "Completed"))
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request != null && request.Status == "Pending")
            {
                request.MechanicUserId = userId;
                request.Status = "In Progress";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request != null && request.MechanicUserId == userId && request.Status == "In Progress")
            {
                request.Status = "Completed";
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ServiceRequestStatusChanged", request.Id, "Completed");
            }

            return RedirectToPage();
        }

    }
}
