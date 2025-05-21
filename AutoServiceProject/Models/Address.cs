using System.ComponentModel.DataAnnotations;

namespace AutoServiceProject.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [Display(Name = "Address Title")]
        public string Title { get; set; } 

        [Required]
        [Display(Name = "Full Address")]
        public string FullAddress { get; set; }
    }
}
