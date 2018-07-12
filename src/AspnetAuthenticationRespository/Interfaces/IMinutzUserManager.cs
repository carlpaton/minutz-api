using Microsoft.AspNetCore.Identity;

namespace AspnetAuthenticationRepository.Interfaces
{
    public interface IMinutzUserManager
    {
        (bool Condition, string message, IdentityUser user) Ensure
            (UserManager<IdentityUser> userManager, string email, string password);
    }
}