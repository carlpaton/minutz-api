﻿using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Minutz.Models.Entities;
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
            (this HttpRequest resquest,ClaimsPrincipal user ,Interface.Services.IAuthenticationService authenticationService)
        {
            var userInfo = authenticationService.LoginFromFromToken(
                    resquest.Headers.First(i => i.Key == "access_token").Value,
                    resquest.Headers.First(i => i.Key == "Authorization").Value,
                    user.Claims.ToList().First(i => i.Type == "exp").Value, "");
            return userInfo;
        }
    }
}