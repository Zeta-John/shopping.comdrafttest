using Microsoft.AspNetCore.Identity;
namespace shoppingcomdraft5.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
    }

}
