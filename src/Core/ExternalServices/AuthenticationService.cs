using Interface.Services;
using Models.Entities;

namespace Core.ExternalServices
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IApplicationSetting _applicationSetting;
    public AuthenticationService(IApplicationSetting applicationSetting)
    {
      _applicationSetting = applicationSetting;
    }

    public AuthRestModel GetUserInfo(string token)
    {
      var result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel>(
        Helper.HttpService.Get($"{_applicationSetting.Authority}userinfo", token));
      return result;
    }
  }
}
