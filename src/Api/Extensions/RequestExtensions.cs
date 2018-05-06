using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Minutz.Models.Message;

namespace Api.Extensions
{
    public static class RequestExtensions
    {
        public static string Token(this HttpRequest request)
        {
            if (!request.Headers.Any())
                throw new NullReferenceException(
                    "There was a problem looking for the token header, as there are no headers for this request");
            if (!request.Headers.Any(i => i.Key == "Authorization"))
                throw new NullReferenceException("There was a problem looking for the Authorization header.");
            if (string.IsNullOrEmpty(request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value))
                throw new NullReferenceException("There was a problem with the token provided, it seems empty.");
            return request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
        }

        public static bool CheckEmail(this string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        
        public static AuthRestModelResponse ExtractAuth
            (this HttpRequest request, ClaimsPrincipal user ,Interface.Services.IAuthenticationService authenticationService)
        {
            var accessTokenKey = "accessToken";
            var authorizationKey = "Authorization";
            var xExp = "xExp";
            
            var accessTokenValue = string.Empty;
            var authorization = string.Empty;
            var expiration = string.Empty;
            Console.WriteLine("Info: ExtractAuth --");
            if (request.Headers != null)
            {
                Console.WriteLine("Has headers");
                foreach (var header in request.Headers.ToList())
                {
                    Console.WriteLine("-------");
                    Console.WriteLine(header.Key);
                    Console.WriteLine("-------");
                    Console.Write(header.Value);
                    Console.WriteLine("-------");
                }
            }

            if (request.Headers != null && request.Headers.Keys.Contains(accessTokenKey))
            {
                accessTokenValue = request.Headers.First(i => i.Key == "accessToken").Value;
            }
            else
            {
                Console.Write("accessToken is missing");
            }

            if (user.Claims.ToList().Any( i => i.Type == "exp") )
            {
                Console.WriteLine("-------");
                expiration = user.Claims.ToList().First(i => i.Type == "exp").Value;
                Console.Write(expiration);
                Console.WriteLine("-------");
            }else
            {
                Console.Write("xExp is missing");
            }
            
            if (request.Headers != null && request.Headers.Keys.Contains(authorizationKey))
            {
                authorization = request.Headers.First(i => i.Key == "Authorization").Value;
            }else
            {
                Console.Write("Authorization is missing");
            }
            
            var userInfo = authenticationService.LoginFromFromToken
                (accessTokenValue, authorization,expiration , "");
            return userInfo;
        }
    }
}