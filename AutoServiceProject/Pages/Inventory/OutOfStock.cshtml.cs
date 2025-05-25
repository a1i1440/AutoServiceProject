using AutoServiceProject.Data;
using AutoServiceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceProject.Pages.Inventory
{
    public class OutOfStockModel : PageModel
    {
        private readonly AppDbContext _context;

        public OutOfStockModel(AppDbContext context)
        {
            _context = context;
        }

        public List<SparePart> Parts { get; set; }

        public void OnGet()
        {
            Parts = _context.Parts
                .Where(p => p.Quantity == 0)
                .ToList();
        }
    }
}
