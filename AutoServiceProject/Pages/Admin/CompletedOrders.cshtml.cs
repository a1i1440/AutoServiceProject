using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin
{
    public class CompletedOrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public CompletedOrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .Where(o => o.Status == "Delivered")
                .ToListAsync();
        }
    }
}
