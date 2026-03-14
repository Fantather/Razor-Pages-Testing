using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Pages_Testing.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Pages.Users
{
    public class ResetPasswordModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        public class ResetPasswordInputModel
        {
            [Required]
            public string UserId { get; set; }

            [Required(ErrorMessage = "Введите новый пароль")]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }
        }

        [BindProperty]
        public ResetPasswordInputModel InputModel { get; set; } = default!;

        public IActionResult OnGet(string userId)
        {
            InputModel.UserId = userId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser? user = await userManager.FindByIdAsync(InputModel.UserId);
            if (user == null)
            {
                return NotFound();
            }


            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, resetToken, InputModel.NewPassword);


            if(result.Succeeded)
            {
                return RedirectToPage("Index");
            }

            foreach(var Error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, Error.Description);
            }
            return Page();
        }
    }
}
