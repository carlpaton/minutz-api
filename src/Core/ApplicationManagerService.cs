using System;
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
      user.Role = "Owner";
      var masterConnectionString = _applicationSetting.CreateConnectionString(
                                                           _applicationSetting.Server,
                                                           "master",
                                                           _applicationSetting.Username,
                                                           _applicationSetting.Password);
      var userConnectionString = _applicationSetting.CreateConnectionString(
                                                           _applicationSetting.Server,
                                                           _applicationSetting.Catalogue,
                                                           _applicationSetting.Username,
                                                           _applicationSetting.Password);
      try
      {
        var schemaCreate = _userRepository.CreateNewSchema(
                                                    user,
                                                    _applicationSetting.Schema,
                                                    userConnectionString,
                                                    masterConnectionString);
        _applicationSetupRepository.CreateSchemaTables(_applicationSetting.Schema, schemaCreate,
                                      _applicationSetting.CreateConnectionString(
                                                                    _applicationSetting.Server,
                                                                    _applicationSetting.Catalogue,
                                                                    _applicationSetting.Username,
                                                                    _applicationSetting.Password));
      }
      catch (Exception ex)
      {
        return false;
      }

      return true;
    }
  }
}
