using System;
using System.Linq;
using AspnetAuthenticationRepository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AspnetAuthenticationRepository
{
    public class MinuzUserManager : IMinutzUserManager
    {
        public (bool Condition, string message, IdentityUser user) Ensure
            (UserManager<IdentityUser> userManager, string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email};
            if (userManager.Users.SingleOrDefault(r => r.Email == email) != null)
                return (true, "Got Existing user.", userManager.Users.Single(r => r.Email == email));
            var create = userManager.CreateAsync(user, password).Result;
            return !create.Succeeded ? (false, string.Join(",", create.Errors) , null) : (true, "Success", user);
        }
    }
}