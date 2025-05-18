namespace AutoServiceProject.Models
{
    public class Mechanic
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SpecializationBrand { get; set; }
        public int Age { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }


}
