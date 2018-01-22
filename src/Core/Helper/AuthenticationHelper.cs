using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
using System.Linq;

namespace Core.Helper
{
  public class AuthenticationHelper
  {
    public enum Role
    {
      User = 1,
      Guest = 2,
      Admin = 3
    }
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
      if(string.IsNullOrEmpty(this.UserInfo.Related))
      {
        this.Instance = instanceRepository.GetByUsername(this.UserInfo.InstanceId,
                                                              applicationSetting.Schema,
                                                              applicationSetting.CreateConnectionString(
                                                                                                        applicationSetting.Server,
                                                                                                        applicationSetting.Catalogue,
                                                                                                        applicationSetting.Username,
                                                                                                        applicationSetting.Password));
        
      }else
      {
        (string instanceId, string meetingId) relatedInstance = this.UserInfo.Related.SplitToList("&", ";").FirstOrDefault(); // This is to be updated to allow multiple
        this.Instance = instanceRepository.GetByUsername(relatedInstance.instanceId,
                                                                      applicationSetting.Schema,
                                                                      applicationSetting.CreateConnectionString(
                                                                                                                applicationSetting.Server,
                                                                                                                applicationSetting.Catalogue,
                                                                                                                applicationSetting.Username,
                                                                                                                applicationSetting.Password));
      }
      this.ConnectionString = applicationSetting.CreateConnectionString(
                                                                          applicationSetting.Server,
                                                                          applicationSetting.Catalogue,
                                                                          this.Instance.Username,
                                                                          this.Instance.Password);
    }
  }
}
