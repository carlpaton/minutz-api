using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AspnetAuthenticationRepository.Interfaces
{
    public interface IMinutzRoleManager
    {
        (bool Condition, string Message, List<string> roles, IdentityUser user) Ensure
            (UserManager<IdentityUser> userManager, string email, string role);

        (bool Condition, string Message, List<string> Roles) Roles
            (UserManager<IdentityUser> userManager, string email);
    }
}