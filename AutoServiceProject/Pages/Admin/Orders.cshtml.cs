using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Where(o => o.Status == "Active")
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .ToListAsync();
        }



        public async Task<IActionResult> OnPostAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
            }

            LoadOrders(); 
            return Page();
        }

        private void LoadOrders()
        {
            Orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.SparePart)
                .ToList();
        }
    }
}
