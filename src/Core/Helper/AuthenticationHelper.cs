using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core.Helper
{
  public class AuthenticationHelper
  {
    public AuthRestModel UserInfo { get; private set; }
    public Instance Instance { get; private set; }
    public string ConnectionString { get; private set; }

    public AuthenticationHelper(string token,
                                IAuthenticationService authenticationService,
                                IInstanceRepository instanceRepository,
                                IApplicationSetting applicationSetting,
                                IUserValidationService userValidationService)
    {
      this.UserInfo = userValidationService.GetUser(authenticationService.GetUserInfo(token).Sub);
      this.Instance = instanceRepository.GetByUsername(this.UserInfo.InstanceId,
                                                      applicationSetting.Schema,
                                                      applicationSetting.CreateConnectionString(
                                                                                                applicationSetting.Server,
                                                                                                applicationSetting.Catalogue,
                                                                                                applicationSetting.Username,
                                                                                                applicationSetting.Password));
      this.ConnectionString = applicationSetting.CreateConnectionString(
                                                                            applicationSetting.Server,
                                                                            applicationSetting.Catalogue,
                                                                            this.Instance.Username,
                                                                            this.Instance.Password);

    }
  }
}
