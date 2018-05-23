using System;
using System.Linq;
using System.Security.Claims;
using Minutz.Models.Entities;

namespace Api.Extensions
{
    public static class IdentityExtensions
    {
        public static AuthRestModel ToRest
            (this ClaimsPrincipal user)
        {
            var email = user.Claims.ToList().SingleOrDefault(i => i.Type == "sub");
            if (email == null) throw new Exception("The user Identity does not contain a sub [email].");
            var instanceId = user.Claims.ToList().SingleOrDefault(i => i.Type == "instanceId");
            if (instanceId == null) throw new Exception("The user Identity does not contain instanceId.");
            var role = user.Claims.ToList().SingleOrDefault(i => i.Type == "roles");
            if (role == null) throw new Exception("The user Identity does not contain role.");
            var name = user.Claims.ToList().SingleOrDefault(i => i.Type == "nickname");
            if (name == null) throw new Exception("The user Identity does not contain name.");
            var picture = user.Claims.ToList().SingleOrDefault(i => i.Type == "picture");
            if (picture == null) throw new Exception("The user Identity does not contain picture.");
            var exp = user.Claims.ToList().SingleOrDefault(i => i.Type == "exp");
            if (exp == null) throw new Exception("The user Identity does not contain exp.");
            var accessToken = user.Claims.ToList().SingleOrDefault(i => i.Type == "access_token");
            if (accessToken == null) throw new Exception("The user Identity does not contain exp.");
            var result = new AuthRestModel
                         {
                             Email = email.Value,
                             Name = name.Value,
                             Picture = picture.Value,
                             Nickname = name.Value,
                             Role = role.Value,
                             InstanceId = instanceId.Value,
                             TokenExpire = exp.Value,
                             Sub = email.Value,
                             AccessToken = accessToken.Value
                         };
            return result;
        }
    }
}
