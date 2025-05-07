using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceProject.Pages.Store
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DetailsModel(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public SparePart SparePart { get; set; }
        public List<Review> Reviews { get; set; }
        public bool CanReview { get; set; }

        [BindProperty]
        public Review NewReview { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            SparePart = await _context.Parts.FindAsync(id);
            if (SparePart == null) return NotFound();

            Reviews = await _context.Reviews
                .Where(r => r.SparePartId == id)
                .Include(r => r.User)
                .ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var hasPurchased = _context.Orders
                    .Any(o => o.UserId == user.Id && o.SparePartId == id);
                var hasReviewed = _context.Reviews
                    .Any(r => r.UserId == user.Id && r.SparePartId == id);

                CanReview = hasPurchased && !hasReviewed;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var hasPurchased = _context.Orders
                .Any(o => o.UserId == user.Id && o.SparePartId == id);
            var hasReviewed = _context.Reviews
                .Any(r => r.UserId == user.Id && r.SparePartId == id);

            if (!hasPurchased || hasReviewed) return RedirectToPage(new { id });

            NewReview.SparePartId = id;
            NewReview.UserId = user.Id;
            NewReview.DatePosted = DateTime.Now;

            _context.Reviews.Add(NewReview);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
        public async Task<IActionResult> OnPostOrderAsync(int id, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var part = await _context.Parts.FindAsync(id);
            if (part == null) return NotFound();

            var order = new Order
            {
                UserId = user.Id,
                SparePartId = id,
                Quantity = quantity,
                Address = user.Address,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Profile/Orders"); 
        }
    }
}