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

        public decimal TotalIncome { get; set; }


        public List<decimal> MonthlyRevenue { get; set; } = new List<decimal>();

        public async Task OnGetAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.Status == "Completed" || o.Status == "Delivered")
                .ToListAsync();

            MonthlyRevenue = Enumerable.Repeat(0m, 12).ToList();

            foreach (var order in orders)
            {
                var date = order.OrderDate;
                int month = date.Month;
                MonthlyRevenue[month - 1] += order.TotalPrice;
            }

            UserCount = await _userManager.GetUsersInRoleAsync("User").ContinueWith(t => t.Result.Count);
            PartCount = await _context.Parts.CountAsync();
            TotalIncome = await _context.Orders
                .Where(o => o.Status == "Completed" || o.Status == "Delivered")
                .SumAsync(o => o.TotalPrice);

            CompletedOrders = orders.Count;
            ActiveOrders = await _context.Orders.CountAsync(o => o.Status == "Active");


            TotalRevenue = TotalIncome;
        }




    }
}
