using Microsoft.AspNetCore.Identity;

namespace Razor_Pages_Testing.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
