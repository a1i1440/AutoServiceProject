namespace AutoServiceProject.Models 
{
public class Order
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int SparePartId { get; set; }
    public SparePart SparePart { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
    public decimal TotalPrice { get; set; }

    }

}
