using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceProject.Pages.Store
{
    public class CategoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public CategoryModel(AppDbContext context)
        {
            _context = context;
        }

        public List<SparePart> Products { get; set; }

        public string CategoryName { get; set; }

        public void OnGet(string categoryName)
        {
            CategoryName = categoryName.Replace("-", " ");
            Products = _context.Parts
                .Where(p => p.Category.ToLower() == CategoryName.ToLower())
                .ToList();
        }
    }
}
