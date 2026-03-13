using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Models.Users;
using Razor_Pages_Testing.Models.UserTasks;

namespace Razor_Pages_Testing.Data
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
