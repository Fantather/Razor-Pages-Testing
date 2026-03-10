using Microsoft.AspNetCore.Identity;

namespace Razor_Pages_Testing.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
