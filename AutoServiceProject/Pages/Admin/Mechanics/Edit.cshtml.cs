using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using MechanicModel = AutoServiceProject.Models.Mechanic;
namespace AutoServiceProject.Pages.Admin.Mechanics
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public MechanicModel Mechanic { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var mechanicInDb = await _context.Mechanics.FindAsync(Mechanic.Id);
            if (mechanicInDb == null)
            {
                return NotFound();
            }

            mechanicInDb.FullName = Mechanic.FullName;
            mechanicInDb.Age = Mechanic.Age;
            mechanicInDb.SpecializationBrand = Mechanic.SpecializationBrand;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Mechanics/Index");
        }
    }
}
