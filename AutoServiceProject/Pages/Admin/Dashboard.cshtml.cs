using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public DashboardModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public int UserCount { get; set; }
        public int PartCount { get; set; }
        public int ActiveOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<SparePart> OutOfStockParts { get; set; } = new();
        public int MechanicCount { get; set; }



        public async Task OnGetAsync()
        {
            var usersInUserRole = await _userManager.GetUsersInRoleAsync("User");
            UserCount = usersInUserRole.Count;
            PartCount = await _context.Parts.CountAsync();
            MechanicCount = await _context.Mechanics.CountAsync();
            ActiveOrders = await _context.Orders.CountAsync(o => o.Status != "Delivered");
            CompletedOrders = await _context.Orders.CountAsync(o => o.Status == "Delivered");
            OutOfStockParts = await _context.Parts
                .Where(p => p.Quantity == 0 || !p.InStock)
                .ToListAsync();

            TotalRevenue = await _context.Orders
                .Where(o => o.Status == "Delivered")
                .SumAsync(o => o.Quantity * o.SparePart.Price);
        }
    }
}
