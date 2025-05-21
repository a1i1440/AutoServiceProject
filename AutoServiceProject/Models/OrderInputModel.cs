using System.ComponentModel.DataAnnotations;

namespace AutoServiceProject.Models
{
    public class OrderInputModel
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
