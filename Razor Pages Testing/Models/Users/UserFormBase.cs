using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Models.Users
{
    public abstract class UserFormBase
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите электронную почту")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите возраст")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
    }
}
