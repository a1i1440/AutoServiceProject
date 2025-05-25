using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoServiceProject.Models;
using System.Linq;
using AutoServiceProject.Data;

namespace AutoServiceProject.Pages.Profile
{
    public class ReceiptModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReceiptModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }

        public IActionResult OnGet(int orderId)
        {
            Order = _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new Order
                {
                    Id = o.Id,
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    Address = o.Address,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    SparePart = new SparePart
                    {
                        Name = o.SparePart.Name,
                        Brand = o.SparePart.Brand
                    }
                })
                .FirstOrDefault();

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
