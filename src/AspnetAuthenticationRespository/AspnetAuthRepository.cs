using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AspnetAuthenticationRespository.Interfaces;
using Interface;
using Interface.Repositories;
using Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Minutz.Models;
using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Models.Auth0Models;

namespace AspnetAuthenticationRespository
{
    public class AspnetAuthRepository : IAuthRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICustomPasswordValidator _customPasswordValidator;
        private readonly IMinutzUserManager _minutzUserManager;
        private readonly IMinutzRoleManager _minutzRoleManager;
        private readonly IApplicationSetting _applicationSetting;

        public AspnetAuthRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IUserRepository userRepository,
            ICustomPasswordValidator customPasswordValidator,
            IMinutzUserManager minutzUserManager,
            IMinutzRoleManager minutzRoleManager,
            IApplicationSetting applicationSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _customPasswordValidator = customPasswordValidator;
            _minutzUserManager = minutzUserManager;
            _minutzRoleManager = minutzRoleManager;
            _applicationSetting = applicationSetting;
        }

        public (bool condition, string message, AuthRestModel value) CreateUser
            (string name, string email, string password, string role, string instanceId)
        {
            if (string.IsNullOrEmpty(email)) return (false, "Please provide as username", null);
            if (string.IsNullOrEmpty(password)) return (false, "Please provide as password", null);
            if (string.IsNullOrEmpty(role)) return (false, "Please provide as role", null);
            
            if (_customPasswordValidator.CheckStrength(password) == PasswordScore.Blank)
            {
                return (false, "Please provide a valid password", null);
            }
            if (_customPasswordValidator.CheckStrength(password) == PasswordScore.Weak)
            {
                return (false, "Please provide a stronger password", null);
            }

            var userResult = _minutzUserManager.Ensure(_userManager, email, password);
            if (!userResult.Condition) return (userResult.Condition, userResult.message, null);
            
            _signInManager.SignInAsync(userResult.user, false);

            var rolesResult = _minutzRoleManager.Ensure(_userManager, email, role);
            if (!rolesResult.Condition) return (rolesResult.Condition, rolesResult.Message, null);
            
            var resultModel = new AuthRestModel
            {
                Sub = email,
                IsVerified = false,
                Email = email,
                Nickname = name,
                InstanceId = $"A_{rolesResult.user.Id}",
                Role = rolesResult.roles.FirstOrDefault()
            };
            return (true, "Success", resultModel);
        }


        /// <summary>
        /// Get the user by email not the token
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AuthRestModelResponse GetUserInfo(string email)
        {
            var result = new AuthRestModelResponse {Condition = false};
            //todo: check the usage
            var userModel = SearchUserByEmail(email);
            result.InfoResponse = new AuthRestModel {Email = userModel.User.email};

            return result;
        }

        public TokenResponse CreateToken(string username, string password)
        {
            var returnObject = new TokenResponse {Condition = false, AuthTokenResponse = new UserResponseModel()};
            var result = _signInManager.PasswordSignInAsync(username, password, false, false).Result;

            if (result.Succeeded)
            {
                var rolesResult = _minutzRoleManager.Ensure(_userManager, username, "User");
                if (!rolesResult.Condition)
                {
                    returnObject.Message = rolesResult.Message;
                    return returnObject;
                }

                var userResponseModel = GenerateJwtToken(username, rolesResult.user, rolesResult.roles);
                returnObject.AuthTokenResponse.access_token = username;
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
            try
            {
                var user = _userManager.Users.SingleOrDefault(i => i.Email == email);
                if (user == null) return result;
                result.Condition = true;
                result.User.email = user.Email;
                result.User.email_verified = user.EmailConfirmed;
                return result;
            }
            catch (ArgumentNullException e)
            {
                result.Condition = false;
                result.Message = e.Message;
                return result;
            }
        }

        public (bool condition, string message, bool value) ValidateUser(string email)
        {
            var result = SearchUserByEmail(email);
            return result.Condition
                ? (result.Condition, result.Message, result.User.email_verified)
                : (result.Condition, result.Message, result.Condition);
        }

        private UserResponseModel GenerateJwtToken(string email, IdentityUser user, IList<string> roles)
        {
            var dbUser = _userRepository.GetUserByEmail(email, "app", _applicationSetting.CreateConnectionString());
            var instanceId = dbUser == null ? $"A_{Guid.NewGuid()}" : dbUser.Identityid;
            var name = dbUser == null ? email : dbUser.FullName;
            var picture = (dbUser == null ? string.Empty : dbUser.ProfilePicture) ?? string.Empty;
            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("nickname", name),
                new Claim("picture", picture),
                new Claim("access_token", email),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim("roles", string.Join(",", roles)),
                new Claim("instanceId", instanceId),
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

        internal int JavascriptTime()
        {
            return (int) DateTime.UtcNow
                .AddDays(1).Minute;
        }
    }
}