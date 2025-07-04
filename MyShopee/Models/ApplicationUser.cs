using Microsoft.AspNetCore.Identity;

namespace MyShopee.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
