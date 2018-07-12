using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspnetAuthenticationRepository.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AspnetAuthenticationRepository
{
    public class MinutzJwtSecurityTokenManager: IMinutzJwtSecurityTokenManager
    {
        public (string token, DateTime expires) JwtSecurityToken
            (string clientSecret, string domain, IEnumerable<Claim> claims)
        {
            if(string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if(string.IsNullOrEmpty(domain)) throw new ArgumentNullException(nameof(domain));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(3);

            var token = new JwtSecurityToken(
                domain,
                domain,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}