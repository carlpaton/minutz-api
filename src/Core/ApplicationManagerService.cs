using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
  public class ApplicationManagerService : IApplicationManagerService
  {
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IApplicationSetting _applicationSetting;
    public ApplicationManagerService(IApplicationSetupRepository applicationSetupRepository,
                                     IUserRepository userRepository,
                                     IApplicationSetting applicationSetting)
    {
      _applicationSetupRepository = applicationSetupRepository;
      _userRepository = userRepository;
      _applicationSetting = applicationSetting;
    }

    public bool StartFullVersion(AuthRestModel user)
    {
      var schemaCreate = _userRepository.CreateNewSchema(user, _applicationSetting.Schema,
                                                         _applicationSetting.CreateConnectionString(
                                                           _applicationSetting.Server,
                                                           _applicationSetting.Catalogue,
                                                           _applicationSetting.Username,
                                                           _applicationSetting.Password));
      return _applicationSetupRepository.CreateSchemaTables(_applicationSetting.Schema, schemaCreate,
                                                         _applicationSetting.CreateConnectionString(
                                                           _applicationSetting.Server,
                                                           _applicationSetting.Catalogue,
                                                           _applicationSetting.Username,
                                                           _applicationSetting.Password));
    }
  }
}
