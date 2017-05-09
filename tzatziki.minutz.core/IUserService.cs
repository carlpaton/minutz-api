using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.core
{
  public interface IUserService
  {
    User CopyPersonToUser(UserProfile user, AppSettings appsettings);
  }
}