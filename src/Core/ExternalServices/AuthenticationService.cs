using Interface.Services;
using Minutz.Models.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Core.ExternalServices
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IApplicationSetting _applicationSetting;
    private IMemoryCache _cache;
    public AuthenticationService(IApplicationSetting applicationSetting, IMemoryCache memoryCache)
    {
      _applicationSetting = applicationSetting;
      _cache = memoryCache;
    }

    public AuthRestModel GetUserInfo(string token)
    {
      AuthRestModel result;
      if (!_cache.TryGetValue(token, out result)){
        var httpResult = Helper.HttpService.Get($"{_applicationSetting.Authority}userinfo", token);
        result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel>(httpResult);
        // Set cache options.
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));
        // Save data in cache.
        _cache.Set(token, result, cacheEntryOptions);
      }
      return result;
    }
  }
}
