using Models.Auth0Models;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IAuthenticationService
  {
    (bool condition, string message, AuthRestModel tokenResponse) CreateUser (
      string name, string username, string email, string password, string role, string invitationInstanceId, string meetingId);

    (bool condition, string message, AuthRestModel infoResponse) Login (
      string username, string password, string instanceId);

    AuthRestModel GetUserInfo(string token);

    AuthRestModel ResetUserInfo(string token);
  }
}
