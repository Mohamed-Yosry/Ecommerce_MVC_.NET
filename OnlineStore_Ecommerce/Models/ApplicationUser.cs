using Microsoft.AspNetCore.Identity;

namespace OnlineStore_Ecommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
