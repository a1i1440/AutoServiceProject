using AutoServiceProject.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [BindProperty]
        public OrderInputModel OrderInput { get; set; }

        public async Task<IActionResult> OnPostEditAsync()
        {
            Console.WriteLine($"📦 ID: {OrderInput?.Id}, Quantity: {OrderInput?.Quantity}, Address: {OrderInput?.Address}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState invalid!");
                return RedirectToPage();
            }

            var existing = await _context.Orders.FindAsync(OrderInput.Id);
            if (existing == null || existing.Status != "Pending")
                return RedirectToPage();

            existing.Quantity = OrderInput.Quantity;
            existing.Address = OrderInput.Address;

            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Updated successfully.");
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .Include(o => o.SparePart)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == user.Id);

            if (order == null || order.Status != "Pending")
                return RedirectToPage();

            order.SparePart.Quantity += order.Quantity;
            order.Status = "Cancelled";

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

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