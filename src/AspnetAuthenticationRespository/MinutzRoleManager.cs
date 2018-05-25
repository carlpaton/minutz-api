using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using AspnetAuthenticationRespository.Interfaces;

namespace AspnetAuthenticationRespository
{
    public class MinutzRoleManager : IMinutzRoleManager
    {
        public (bool Condition, string Message, List<string> roles, IdentityUser user) Ensure
            (UserManager<IdentityUser> userManager, string email, string role)
        {
            var appUser = userManager.Users.SingleOrDefault(r => r.Email == email);
            if(appUser == null) throw new Exception("User does not exist.");
            try
            {
                var rolesCheck = userManager.GetRolesAsync(appUser).Result;
                if (!rolesCheck.Contains(role))
                {
                    var createRole = userManager.AddToRoleAsync(appUser, role).Result;
                }
                var roles = userManager.GetRolesAsync(appUser).Result;
                return  (true, "", roles.ToList(), appUser);
            }
            catch (Exception e)
            {
                return (false, e.Message, null, null);
            }
        }
    }
}