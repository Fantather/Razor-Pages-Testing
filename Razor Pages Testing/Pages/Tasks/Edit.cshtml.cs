using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.UserTasks;

namespace Razor_Pages_Testing.Pages.Tasks
{
    public class EditModel(ApplicationDbContext dbContext) : PageModel
    {
        public class UserTaskInputModel : InputUserTaskBase
        {
            public int Id { get; init; }
        }

        [BindProperty]
        public UserTaskInputModel InputModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserTask? task = await dbContext.UserTasks
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(task => task.Id == id);

            if(task is null)
            {
                return NotFound();
            }

            InputModel = new UserTaskInputModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsDone = task.IsDone
            }; 

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            if(ModelState.IsValid)
            {
                return Page();
            }

            UserTask? task = await dbContext.UserTasks.FirstOrDefaultAsync(t => t.Id == id);
            if(task is null)
            {
                return NotFound();
            }

            task.Title = InputModel.Title;
            task.Description = InputModel.Description;
            task.IsDone = InputModel.IsDone;
            await dbContext.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
