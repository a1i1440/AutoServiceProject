using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Inventory
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public SparePart Part { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Part = await _context.Parts.FindAsync(id);
            if (Part == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var part = await _context.Parts.FindAsync(id);

            if (part == null)
            {
                return NotFound();
            }

            _context.Entry(part).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }
}
