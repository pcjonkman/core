using System.Threading.Tasks;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Core.Authorization
{
    public class ContactAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        protected override Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          OperationAuthorizationRequirement requirement,
          User resource
        )
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            // Administrators can do anything.
            // if (context.User.IsInRole(Constants.ContactAdministratorsRole))
            if (context.User.IsInRole(Roles.Admin))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
