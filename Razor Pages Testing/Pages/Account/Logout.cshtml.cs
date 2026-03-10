using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Pages_Testing.Models;

namespace Razor_Pages_Testing.Pages.Account
{
    [Authorize]
    public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
    {
        // Выход из системы изменяет состояние приложения, поэтому по стандартам HTTP это действие должно выполняться только через POST-запрос
        // А ещё, form для Post запроса добавляет скрытый Anti-Forgery Token, так что даже гипотетически нельзя будет выкинуть пользователя из аккаунта из-за пределов сайта
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            await signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Users/Index");
        }
    }
}