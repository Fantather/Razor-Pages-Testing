using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Pages.Tasks
{
    public class IndexModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : PageModel
    {
        public record class UserTaskViewModel(
            [Display(Name = "Номер")]
            int Id,

            [Display(Name = "Название задачи")]
            string Title,

            [Display(Name = "Подробное описание")]
            string Description,

            [Display(Name = "Статус")]
            bool IsDone
        );

        [BindProperty]
        public List<UserTaskViewModel> UserTasks { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUserId = userManager.GetUserId(User);

            UserTasks = await dbContext
                            .UserTasks
                            .AsNoTracking()
                            .Where(task => task.OwnerId == currentUserId)
                            .Select(task => new UserTaskViewModel(task.Id, task.Title, task.Description, task.IsDone))
                            .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var currentUserId = userManager.GetUserId(User);
            var result = await dbContext.UserTasks
                        .Where(task => task.Id == id && task.OwnerId == currentUserId)
                        .ExecuteDeleteAsync();

            return RedirectToPage();
        }
    }
}
