using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
using System.Linq;

namespace Core.Helper
{
  public class AuthenticationHelper
  {
    public const string User = "User";
    public const string Guest = "Guest";
    public const string Admin = "Admin";

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

      if (string.IsNullOrEmpty(this.UserInfo.Related))// this will use the defasult user instance id, this is if the user is a owner
      {

        this.Instance = instanceRepository.GetByUsername(this.UserInfo.InstanceId,
                                                              applicationSetting.Schema,
                                                              applicationSetting.CreateConnectionString(
                                                                                                        applicationSetting.Server,
                                                                                                        applicationSetting.Catalogue,
                                                                                                        applicationSetting.Username,
                                                                                                        applicationSetting.Password));

      }
      else
      {
        if (this.UserInfo.Related.Contains("&"))
        {
          string relatedstring = this.UserInfo.Related;
          if (this.UserInfo.Related.Contains('|'))
            relatedstring = this.UserInfo.Related.TupleSplit().value;

          (string instanceId, string meetingId) relatedInstance = relatedstring.SplitToList("&", ";").FirstOrDefault(); // This is to be updated to allow multiple
          this.Instance = instanceRepository.GetByUsername(relatedInstance.instanceId,
                                                                        applicationSetting.Schema,
                                                                        applicationSetting.CreateConnectionString(
                                                                                                                  applicationSetting.Server,
                                                                                                                  applicationSetting.Catalogue,
                                                                                                                  applicationSetting.Username,
                                                                                                                  applicationSetting.Password));
        }
        else
        {
          this.Instance = instanceRepository.GetByUsername(this.UserInfo.Name,
                                                             applicationSetting.Schema,
                                                             applicationSetting.CreateConnectionString(
                                                                                                       applicationSetting.Server,
                                                                                                       applicationSetting.Catalogue,
                                                                                                       applicationSetting.Username,
                                                                                                       applicationSetting.Password));
        }

      }
      if (this.Instance == null)
      {
        this.ConnectionString = applicationSetting.CreateConnectionString(
                                                                    applicationSetting.Server,
                                                                    applicationSetting.Catalogue,
                                                                    applicationSetting.Username,
                                                                    applicationSetting.Password);
      }
      else
      {
        this.ConnectionString = applicationSetting.CreateConnectionString(
                                                                    applicationSetting.Server,
                                                                    applicationSetting.Catalogue,
                                                                    this.Instance.Username,
                                                                    this.Instance.Password);
      }

    }
  }
}
