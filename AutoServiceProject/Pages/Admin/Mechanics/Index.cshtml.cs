using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MechanicModel = AutoServiceProject.Models.Mechanic;
namespace AutoServiceProject.Pages.Admin.Mechanics
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<MechanicModel> Mechanics { get; set; }

        public async Task OnGetAsync()
        {
            Mechanics = await _context.Mechanics.ToListAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            MechanicModel mechanic = await _context.Mechanics.FindAsync(id);
            if (mechanic != null)
            {
                _context.Mechanics.Remove(mechanic);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

    }
}
