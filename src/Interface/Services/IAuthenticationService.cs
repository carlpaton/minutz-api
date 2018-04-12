using Models.Auth0Models;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IAuthenticationService
  {
    (bool condition, string message, AuthRestModel tokenResponse) CreateUser (
      string name, string email, string password, string role, string invitationInstanceId, string meetingId);

    (bool condition, string message, AuthRestModel infoResponse) Login (
      string username, string password, string instanceId);

    (bool condition, string message, AuthRestModel infoResponse) LoginFromFromToken(
      string access_token, string id_token, string expires_in, string instanceId = null);

    AuthRestModel GetUserInfo(string token);

    AuthRestModel ResetUserInfo(string token);
  }
}
