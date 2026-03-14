using Microsoft.AspNetCore.Authorization;
using Razor_Pages_Testing.Models.UserTasks;
using System.Security.Claims;

namespace Razor_Pages_Testing.Authorization
{
    public class UserTaskOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class ActicleAuthorHandler : AuthorizationHandler<UserTaskOwnerRequirement, UserTask>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserTaskOwnerRequirement requirement, UserTask resource)
        {
            var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(currentUserId == resource.OwnerId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
