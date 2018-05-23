using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Interface.Repositories;
using Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Models.Auth0Models;
using Remotion.Linq.Clauses.ResultOperators;

namespace AspnetAuthenticationRespository
{
    public class AspnetAuthRepository : IAuthRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationSetting _applicationSetting;

        public AspnetAuthRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IApplicationSetting applicationSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _applicationSetting = applicationSetting;
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
                
                //var token =  GenerateJwtToken(email, user, roles);

                var resultModel = new AuthRestModel
                                  {
                                      IsVerified = false,
                                      Email = email,
                                      Nickname = name,
                                      InstanceId = $"A_{appUser?.Id}",
                                      Role = roles.FirstOrDefault()
                                  };
                return (true,"Success", resultModel);
            }
            return (false, string.Join(",",result.Errors), null);
        }

        private UserResponseModel GenerateJwtToken(string email, IdentityUser user, IList<string> roles)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Sub, email),
                             new Claim(JwtRegisteredClaimNames.Jti, jti ),
                             new Claim("roles", string.Join(",", roles)),
                             new Claim(ClaimTypes.NameIdentifier, user.Id)
                         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSetting.ClientSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(3);

            var token = new JwtSecurityToken(
                _applicationSetting.AuthorityDomain,
                _applicationSetting.AuthorityDomain,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var userModel = new UserResponseModel
                    {
                        access_token = tokenString,
                        expires_in = expires.ToString(CultureInfo.CurrentCulture),
                        id_token = jti,
                        scope = string.Join(",", roles),
                        token_type = "aspnet"
                    };
            return userModel;
        }

        public AuthRestModelResponse GetUserInfo(string token)
        {
            var result = new AuthUserQueryResponse {Condition = false, Message = string.Empty, User = new UserQueryModel()};
            throw new NotImplementedException();
        }

        public TokenResponse CreateToken(string username, string password)
        {
            var returnObject = new TokenResponse { Condition = false};
            var result =  _signInManager.PasswordSignInAsync(username, password, false, false).Result;
            
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == username);
                var rolesCheck = _userManager.GetRolesAsync(appUser).Result;
                if (!rolesCheck.Contains("User"))
                {
                    var createRole =  _userManager.AddToRoleAsync(appUser, "User").Result;
                }

                var roles = _userManager.GetRolesAsync(appUser).Result;
                var userResponseModel = GenerateJwtToken(username, appUser, roles);
                returnObject.AuthTokenResponse = userResponseModel;
                returnObject.Condition = true;
                return returnObject;
            }

            if (result.IsLockedOut)
            {
                returnObject.Message = "It appears that you are locked out.";
            }

            if (result.IsNotAllowed)
            {
                returnObject.Message = "It appears tha you are not allowed.";
            }

            if (result.RequiresTwoFactor)
            {
                returnObject.Message = "Your authentication requires two factor authentication.";
            }

            return returnObject;
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