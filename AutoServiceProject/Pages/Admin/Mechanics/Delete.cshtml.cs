using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin.Mechanics
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mechanic Mechanic { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Mechanic = await _context.Mechanics.FindAsync(id);

            if (Mechanic == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mechanic = await _context.Mechanics.FindAsync(Mechanic.Id);
            if (mechanic == null)
            {
                return NotFound();
            }

            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Mechanics/Index");
        }
    }
}
