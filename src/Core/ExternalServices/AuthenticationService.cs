using System;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Minutz.Models.Entities;
using Models.Auth0Models;

namespace Core.ExternalServices
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IApplicationSetting _applicationSetting;
    private readonly IAuth0Repository _auth0Repository;
    private IMemoryCache _cache;
    public AuthenticationService (
      IApplicationSetting applicationSetting, IMemoryCache memoryCache, IAuth0Repository auth0Repository)
    {
      this._applicationSetting = applicationSetting;
      this._cache = memoryCache;
      this._auth0Repository = auth0Repository;
    }

    public (bool condition, string message, UserResponseModel tokenResponse) Login (
      string email, string password)
    {
      return this._auth0Repository.CreateToken(email,password);
    }

    public AuthRestModel ResetUserInfo (string token)
    {
      this._cache.Remove (token);
      return this.GetUserInfo (token);
    }

    public AuthRestModel GetUserInfo (string token)
    {
      AuthRestModel result;
      if (!_cache.TryGetValue (token, out result))
      {
        var httpResult = Helper.HttpService.Get ($"{_applicationSetting.Authority}userinfo", token);
        result = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel> (httpResult);
        // Set cache options.
        var cacheEntryOptions = new MemoryCacheEntryOptions ()
          // Keep in cache for this time, reset time if accessed.
          .SetSlidingExpiration (TimeSpan.FromMinutes (10));
        // Save data in cache.
        _cache.Set (token, result, cacheEntryOptions);
      }
      return result;
    }
  }
}