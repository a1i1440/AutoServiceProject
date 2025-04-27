using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Store
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<SparePart> Products { get; set; } = new List<SparePart>();

        [BindProperty(SupportsGet = true)]
        public string Brand { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Address { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        public void OnGet()
        {
            var query = _context.Parts.AsQueryable();

            if (!string.IsNullOrEmpty(Brand))
            {
                query = query.Where(p => p.Brand.Contains(Brand));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(p => p.Category.Contains(Category));
            }

            Products = query.ToList();
        }

        public async Task<IActionResult> OnPostOrderAsync(int id, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null || quantity <= 0)
                return Page();

            var user = await _userManager.GetUserAsync(User); 

            if (user == null || string.IsNullOrEmpty(user.Address))
            {
                TempData["ErrorMessage"] = "Please complete your address in your profile first.";
                return RedirectToPage("/Profile/Edit");
            }

            var order = new Order
            {
                SparePartId = id,
                Quantity = quantity,
                UserId = user.Id,
                TotalPrice = part.Price * quantity,
                Status = "Active",
                OrderDate = DateTime.Now,
                Address = user.Address
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}
