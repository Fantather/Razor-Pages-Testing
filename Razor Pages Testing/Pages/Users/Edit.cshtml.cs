using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Pages.Users
{
    [Authorize]
    public class EditModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        public class Input : UserFormBase
        {
            [Required]
            public string Id { get; set; } = string.Empty;
        }

        [BindProperty]
        public Input UserUpdate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UserUpdate = new Input
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Age = user.Age
            };

            return Page();
        }

        // Изменённый объект не передаётся в параметры, данные сами попадут в свойство UserUpdate благодаря [BindProperty]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByIdAsync(UserUpdate.Id);

            if (user is null)
            {
                return NotFound();
            }

            user.UserName = UserUpdate.UserName;
            user.Email = UserUpdate.Email;
            user.Age = UserUpdate.Age;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // На пример если почта занята или имя невалидно
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
