using Razor_Pages_Testing.Models.Users;

namespace Razor_Pages_Testing.Models.UserTasks
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; } = false;
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
