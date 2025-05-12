namespace AutoServiceProject.Models
{
    public class CartItem
    {
        public int SparePartId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
