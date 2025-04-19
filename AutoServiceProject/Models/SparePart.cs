using System.ComponentModel.DataAnnotations;

namespace AutoServiceProject.Models
{
    public class SparePart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
    }


}
