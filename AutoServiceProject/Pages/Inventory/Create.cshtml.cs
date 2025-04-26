using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AutoServiceProject.Pages.Inventory
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public SparePart Part { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

               
                Part.ImageUrl = "/images/" + fileName;
            }

            _context.Parts.Add(Part);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
