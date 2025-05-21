using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin
{
    public class PendingOrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public PendingOrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        [BindProperty]
        public int OrderId { get; set; }

        [BindProperty]
        public string NewStatus { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .Where(o => o.Status == "Pending")
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            var order = await _context.Orders.FindAsync(OrderId);
            if (order == null)
                return NotFound();

            order.Status = NewStatus;
            await _context.SaveChangesAsync();

            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .Where(o => o.Status == "Pending")
                .ToListAsync();

            return Page();
        }
    }
}
