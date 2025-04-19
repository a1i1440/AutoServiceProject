using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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



        public List<SparePart> Parts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Brand { get; set; }

        public void OnGet()
        {
            var query = _context.Parts.AsQueryable();

            if (!string.IsNullOrEmpty(Brand))
            {
                query = query.Where(p => p.Brand == Brand);
            }

            Parts = query.ToList();
        }
        public async Task<IActionResult> OnPostOrderAsync(int id, int quantity)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part == null || quantity <= 0 || !User.Identity.IsAuthenticated)
                return Page();

            var userId = _userManager.GetUserId(User);

            var order = new Order
            {
                SparePartId = id,
                Quantity = quantity,
                UserId = userId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}
