using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin
{
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
                .Include(r => r.Mechanic)
                .Include(r => r.User)
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostUpdateStatusAsync(int RequestId, string NewStatus)
        {
            var request = await _context.ServiceRequests.FindAsync(RequestId);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = NewStatus;
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


    }
}
