using Api.Models;
using Interface.Services;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class LoginValidationRequestExtension
    {
        public static (bool Condition, int Code ,string Message) Validate
            (this HttpRequest request, LoginModel user, ILogService logservice)
        {
            if (string.IsNullOrEmpty (user.username))
            {
                logservice.Log (Minutz.Models.LogLevel.Info, "The request did not have a username.");
                return (false, 404, "please provide a valid username or password");
            }

            if (string.IsNullOrEmpty (user.password))
            {
                logservice.Log (Minutz.Models.LogLevel.Info, "The request did not have a password.");
                return (false, 404, "please provide a valid username or password");
            }

            return (true, 200, "Success");
        }
    }
}