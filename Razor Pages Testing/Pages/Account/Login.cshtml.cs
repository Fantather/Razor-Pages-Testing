using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Pages_Testing.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Pages.Account
{
    public class LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : PageModel
    {
        public class LoginInputModel
        {
            [Required(ErrorMessage = "Введите электронную почту")]
            [EmailAddress(ErrorMessage = "Неверный формат почты")]
            [Display(Name = "Электронная почта")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Запомнить меня?")]
            public bool RememberMe { get; set; }
        }


        [BindProperty]
        public LoginInputModel Input { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; } // Что бы перенаправить пользователя туда, откуда он сюда попал


        public IActionResult OnGet()
        {
            // Аутентифицированного пользователя перенаправляем отсюда
            if(User.Identity is not null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Users/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser? user = await userManager.FindByEmailAsync(Input.Email);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Введён не верный email или пароль");
                return Page();
            }

            var result = await signInManager.PasswordSignInAsync(user.UserName!, Input.Password, isPersistent: Input.RememberMe, lockoutOnFailure: true);


            if (result.Succeeded)
            {
                // Защита от атак типа Open Redirect
                if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToPage("/Users/Index");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Слишком много попыток, повторите позже");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Неверный email или пароль");
            return Page();
        }
    }
}
