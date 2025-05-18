using AutoServiceProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ServiceRequest
{
    public int Id { get; set; }

    [Required]
    public string CarBrand { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string SelectedService { get; set; }

    public int SelectedMechanicId { get; set; }

    [ForeignKey("SelectedMechanicId")]
    public Mechanic Mechanic { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string? MechanicUserId { get; set; }
    public AppUser? MechanicUser { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime RequestDate { get; set; } = DateTime.Now; 
}
