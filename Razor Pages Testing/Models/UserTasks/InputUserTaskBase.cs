using System.ComponentModel.DataAnnotations;

namespace Razor_Pages_Testing.Models.UserTasks
{
    public class InputUserTaskBase
    {
        [Required]
        [MinLength(2)]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Выполненность")]
        public bool IsDone { get; set; } = false;
    }
}
