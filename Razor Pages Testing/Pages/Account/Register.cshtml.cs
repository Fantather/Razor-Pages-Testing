using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Pages.Account
{
    public class RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : PageModel
    {
        public class RegisterInputModel : UserFormBase
        {
            [Required(ErrorMessage = "Пароль обязателен")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Подтверждение пароля обязательно")]
            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        [BindProperty]
        public RegisterInputModel InputModel { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = InputModel.UserName,
                Email = InputModel.Email,
                Age = InputModel.Age
            };

            IdentityResult result = await userManager.CreateAsync(user, InputModel.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("Index");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
