namespace AutoServiceProject.Models
{
    public class Customer
    {
        public int Id { get; set; } 

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool IsPremium { get; set; }
        public string? UserId { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
