using Interface.Repositories;
using Interface.Services;

namespace Core
{
  public class UserValidationService: IUserValidationService
  {
    private readonly IUserRepository _userRepository;
    private readonly IApplicationSetting _applicationSetting;
    public UserValidationService(IUserRepository userRepository, 
                                 IApplicationSetting applicationSetting)
    {
      _userRepository = userRepository;
      _applicationSetting = applicationSetting;
    }

    public bool IsNewUser(string authUserId)
    {
      return _userRepository.CheckIfNewUser(authUserId, _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(_applicationSetting.Server, 
                                                   _applicationSetting.Catalogue, 
                                                   _applicationSetting.Username, 
                                                   _applicationSetting.Password));
    }

    public string CreateAttendee(string authUserId)
    {
      return _userRepository.CreateNewUser(authUserId, _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                                                   _applicationSetting.Catalogue,
                                                   _applicationSetting.Username,
                                                   _applicationSetting.Password));
    }
  }
}
