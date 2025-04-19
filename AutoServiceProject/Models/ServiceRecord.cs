using AutoServiceProject.Models;

public class ServiceRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
