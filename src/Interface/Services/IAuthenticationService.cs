using Models.Auth0Models;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IAuthenticationService
  {
    (bool condition, string message, UserResponseModel tokenResponse) Login (
      string email, string password);
    AuthRestModel GetUserInfo(string token);

    AuthRestModel ResetUserInfo(string token);
  }
}
