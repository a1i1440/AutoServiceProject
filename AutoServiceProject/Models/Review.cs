using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceProject.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int SparePartId { get; set; }
        public SparePart SparePart { get; set; }
    }
}
