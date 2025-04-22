using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Inventory
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) => _context = context;

        public SparePart Part { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Part = await _context.Parts.FindAsync(id);
            if (Part == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
