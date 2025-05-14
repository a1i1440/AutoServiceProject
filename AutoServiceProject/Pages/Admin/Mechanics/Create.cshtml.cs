using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Admin.Mechanics
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mechanic Mechanic { get; set; }
        public List<string> CarBrands { get; set; }
        public void OnGet()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/car-brands.json");
            var json = System.IO.File.ReadAllText(jsonPath);

            CarBrands = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Mechanics.Add(Mechanic);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Mechanics/Index");
        }

    }
}
