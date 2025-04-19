using AutoServiceProject.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceProject.Pages.Profile
{
    [Authorize(Roles = "User")]
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrdersModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Orders = await _context.Orders
                .Include(o => o.SparePart)
                .Where(o => o.UserId == user.Id)
                .ToListAsync();
        }
    }
}
