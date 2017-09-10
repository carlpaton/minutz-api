namespace Interface.Services
{
  public interface IUserValidationService
  {
    bool IsNewUser(string authUserId);

    string CreateAttendee(string authUserId);
  }
}
