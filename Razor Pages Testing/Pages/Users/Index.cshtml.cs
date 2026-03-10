using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Pages_Testing.Data;
using Razor_Pages_Testing.Models.Users;

namespace Razor_Pages_Testing.Pages.Users
{
    [Authorize]
    public class IndexModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : PageModel
    {
        public record class UserDto(string Id, string UserName, string Email, int Age);


        public IList<UserDto> Users { get; set; } = default!; // Просто затычка


        // Атрибут TempData нужен, что бы сохранить значение этой переменной на один запрос, наш метод заканчивается перенаправлением, но в значение это переменной останется
        // В Razor Pages обычно вы просто используете механизм ModelState.Errors для вывода ошибок, как и в Razor View
        // Но конкретно здесь так сделать не получится, потому что при нажатии кнопки удалить вызывается OnPostDeleteAsync, он не генерирует список пользователей, который ожидает страница
        // Так что всё сломается, что бы этого не случилось после удаления мы перенаправляем пользователя обратно на метод OnGetAsync, по сути делая второй запрос на сервер
        // А что бы сообщение об ошибке выжило между запросами используем TempData
        [TempData]
        public string? ErrorMessage { get; set; }


        public async Task OnGetAsync()
        {
            Users = await dbContext.Users
                .AsNoTracking()
                .Select(user => new UserDto(
                    user.Id,
                    user.UserName ?? string.Empty,
                    user.Email ?? string.Empty,
                    user.Age))
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                ErrorMessage = "Пользователь уже был удален или не найден";
                return RedirectToPage();
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                // В реальном проекте здесь ещё бы логгирование сделали
                ErrorMessage = "Произошла ошибка при удалении пользователя";
            }

            // Перенаправляет на страницу, которой принадлежит модель (на Index в нашем случае)
            return RedirectToPage();
        }
    }
}
