using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Interface.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace AspnetAuthenticationRespository
{
    public class AspnetAuthRepository : IAuthRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AspnetAuthRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public (bool condition, string message, AuthRestModel value) CreateUser
            (string name, string email, string password, string role, string instanceId)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result =  _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, false);
                
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == email);
                var rolesCheck = _userManager.GetRolesAsync(appUser).Result;
                if (string.IsNullOrEmpty(instanceId))
                {
                    if (!rolesCheck.Contains("User"))
                    {
                        var createRole =  _userManager.AddToRoleAsync(appUser, "User").Result;
                    }
                }
                else
                {
                    if (!rolesCheck.Contains("Guest"))
                    {
                        var createRole =  _userManager.AddToRoleAsync(appUser, "Guest").Result;
                    }
                }
                var roles = _userManager.GetRolesAsync(appUser).Result;
                
                var token =  GenerateJwtToken(email, user, roles);

                var resultModel = new AuthRestModel
                                  {
                                      IsVerified = false,
                                      Email = email,
                                      Nickname = name,
                                      InstanceId = $"A_{appUser.Id}",
                                      Role = roles.FirstOrDefault()
                                  };
                return (true,"Success", resultModel);
            }
            return (false, string.Join(",",result.Errors), null);
        }
        private object GenerateJwtToken(string email, IdentityUser user, IList<string> roles)
        {
            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Sub, email),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             new Claim("roles", string.Join(",", roles)),
                             new Claim(ClaimTypes.NameIdentifier, user.Id)
                         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthRestModelResponse GetUserInfo(string token)
        {
            var result = new AuthUserQueryResponse {Condition = false, Message = string.Empty, User = new UserQueryModel()};
            throw new NotImplementedException();
        }

        public TokenResponse CreateToken(string username, string password)
        {
            throw new NotImplementedException();
        }

        public AuthUserQueryResponse SearchUserByEmail(string email)
        {
            var result =
                new AuthUserQueryResponse {Condition = false, Message = string.Empty, User = new UserQueryModel()};
            throw new NotImplementedException();
        }

        public (bool condition, string message, bool value) ValidateUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}