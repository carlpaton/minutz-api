using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AspnetAuthenticationRepository.Interfaces;

namespace AspnetAuthenticationRepository
{
    public class MinutzClaimManager: IMinutzClaimManager
    {
        public List<Claim> CreateClaims
            (string email, string picture, string name, IEnumerable<string> roles, string instanceId, string userId)
        {
            if(string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if(string.IsNullOrEmpty(picture)) throw new ArgumentNullException(nameof(picture));
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(instanceId)) throw new ArgumentNullException(nameof(instanceId));
            if(string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Sub, instanceId),
                             new Claim("nickname", name),
                             new Claim("picture", picture),
                             new Claim("access_token", email),
                             new Claim(JwtRegisteredClaimNames.Jti, jti),
                             new Claim("roles", string.Join(",", roles)),
                             new Claim("instanceId", instanceId),
                             new Claim(ClaimTypes.NameIdentifier, userId)
                         };
            return claims;
        }
    }
}