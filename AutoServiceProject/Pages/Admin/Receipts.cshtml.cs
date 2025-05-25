using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReceiptsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReceiptsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .Where(o => o.Status == "Completed" || o.Status == "Cancelled")
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
