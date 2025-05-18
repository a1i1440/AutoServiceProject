using AutoServiceProject.Data;
using AutoServiceProject.Hubs;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<AppHub> _hubContext;

        public OrdersModel(AppDbContext context, IHubContext<AppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .Where(o => o.Status == "Active")
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("OrderStatusChanged", new
                {
                    OrderId = order.Id,
                    NewStatus = newStatus
                });
            }

            await OnGetAsync(); 
            return Page();
        }
    }
}
