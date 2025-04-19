using Microsoft.AspNetCore.Identity;

namespace AutoServiceProject.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsPremium { get; set; }
    }

}
