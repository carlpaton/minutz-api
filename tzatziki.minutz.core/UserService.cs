using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.core
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public User CopyPersonToUser(UserProfile user, AppSettings appsettings)
    {
      return _userRepository.Get(user, appsettings.ConnectionStrings.AzureConnection, user.InstanceId.ToSchemaString());
    }
  }
}