
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IAuthenticationService
  {
    AuthRestModel GetUserInfo(string token);
  }
}
