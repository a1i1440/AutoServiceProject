using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Inventory
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SparePart Part { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Part = await _context.Parts.FindAsync(id);
            if (Part == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = await _context.Parts.FindAsync(Part.Id);
            if (existing == null)
                return NotFound();

            existing.Name = Part.Name;
            existing.Brand = Part.Brand;
            existing.Category = Part.Category;
            existing.Quantity = Part.Quantity;
            existing.Price = Part.Price;
            existing.InStock = Part.InStock;

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
