using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.UserTasks;

namespace Razor_Pages_Testing.Pages.Tasks
{
    public class CreateModel(ApplicationDbContext dbContext) : PageModel
    {
        public class CreateUserTaskInputModel : InputUserTaskBase { }


        [BindProperty]
        public CreateUserTaskInputModel InputModel { get; set; } = default!;
        

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await dbContext.UserTasks.AddAsync(new UserTask
            {
                Title = InputModel.Title,
                Description = InputModel.Description,
                IsDone = InputModel.IsDone
            });
            await dbContext.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
