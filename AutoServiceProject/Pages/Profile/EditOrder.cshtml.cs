using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Profile
{
    [Authorize(Roles = "User")]
    public class EditOrderModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditOrderModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null || Order.Status != "Pending")
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = await _context.Orders.FindAsync(Order.Id);

            if (existing == null || existing.Status != "Pending")
                return NotFound();

            existing.Quantity = Order.Quantity;
            existing.Address = Order.Address;

            await _context.SaveChangesAsync();
            return RedirectToPage("/Profile/Orders");
        }
    }
}
