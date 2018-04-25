using Models.Auth0Models;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services
{
  public interface IAuthenticationService
  {
    (bool condition, string message, AuthRestModel tokenResponse) CreateUser (
      string name, string email, string password, string role, string invitationInstanceId, string meetingId);

     AuthRestModelResponse LoginFromLoginForm 
       (string username, string password, string instanceId);

    AuthRestModelResponse LoginFromFromToken(
      string access_token, string id_token, string expires_in, string instanceId = null);

    AuthRestModelResponse GetUserInfo(string token);

    AuthRestModelResponse ResetUserInfo(string token);
  }
}
