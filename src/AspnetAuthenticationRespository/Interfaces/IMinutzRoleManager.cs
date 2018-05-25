using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AspnetAuthenticationRespository.Interfaces
{
    public interface IMinutzRoleManager
    {
        (bool Condition, string Message, List<string> roles, string userId) Ensure
            (UserManager<IdentityUser> userManager, string email, string role);
    }
}