using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IUserValidationService
  {
    bool IsNewUser(string authUserId, string referenceKey);

    string CreateAttendee(AuthRestModel authUserId, string referenceKey);

    AuthRestModel GetUser(string auth0UserId);
  }
}
