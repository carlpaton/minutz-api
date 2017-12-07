using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IUserValidationService
  {
    bool IsNewUser(string authUserId);

    string CreateAttendee(AuthRestModel authUserId);

    AuthRestModel GetUser(string authUserId);
  }
}
