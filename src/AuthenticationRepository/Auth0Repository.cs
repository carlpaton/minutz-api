using System;
using AuthenticationRepository.Extensions;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Models.Auth0Models;

namespace AuthenticationRepository
{
  public class Auth0Repository : IAuth0Repository
  {
    private string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";
    private readonly IHttpService _httpService;
    private readonly ILogService _logService;
    private IMemoryCache _cache;
    private readonly IApplicationSetting _applicationSetting;

    public Auth0Repository (
      ILogService logService, IMemoryCache memoryCache, IApplicationSetting applicationSetting)
    {
      _httpService = new HttpService ();
      _logService = logService;
      _cache = memoryCache;
      _applicationSetting = applicationSetting;
    }
    
    public (bool condition, string message, AuthRestModel value) CreateUser (
      string name, string username, string email, string password, string role, string instanceId)
    {
      var requestBody = new UserRequestModel
        {
          client_id = _applicationSetting.ClientId,
            email = email,
            username = username,
            password = password,
            connection = _applicationSetting.AuthorityConnection
        }.Prepare (instanceId, name, role)
        .ToJSON ().ToStringContent ();
      
      var createResult = _httpService.Post ($"{_applicationSetting.Authority}dbconnections/signup", requestBody);
      
      if (!createResult.condition)
      {
        return (createResult.condition, "There was a issue creating the user.", null);
      }
      
      var resultObject = createResult.result.ToUserCreateResponseModelModel ();
      
      var result = new AuthRestModel
      {
        IsVerified = resultObject.email_verified,
        Email = email,
        Nickname = name,
        InstanceId = resultObject.user_metadata.instance,
        Role = resultObject.user_metadata.role
      };
      
      (bool condition, string message, UserResponseModel tokenResponse) tokenResult = this.CreateToken (username, password);
      
      if (tokenResult.condition)
      {
        (bool condition, string message, AuthRestModel infoResponse) userInfoResult = this.GetUserInfo (tokenResult.tokenResponse.access_token);
        if (userInfoResult.condition)
        {
          result.Sub = userInfoResult.infoResponse.Sub;
          result.Picture = userInfoResult.infoResponse.Picture;
        }
      }

      return (createResult.condition, createResult.result, result);
    }

    public (bool condition, string message, UserQueryModel value)  SearchUserByEmail
      (string email)
    {
      var emailEncoded = System.Net.WebUtility.UrlEncode(email);
      var url = $"{_applicationSetting.Authority}/api/v2/users-by-email?email={emailEncoded}";
      var httpResult = _httpService.Get (url, _applicationSetting.AuthorityManagmentToken);
      if (!httpResult.condition)
      {
        _logService.Log (Minutz.Models.LogLevel.Exception, $"(bool condition, string message, UserQueryModel value)  SearchUserByEmail -> there was a issue getting the details from auth0");
        return (false, "There was a issue getting the user information.", null);
      }
      var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserQueryModel> (httpResult.result);
      return (httpResult.condition, "Success", result);
    }

    public (bool condition, string message, bool value) ValidateUser 
      (string email)
    {
      var result = SearchUserByEmail(email);
      return result.condition 
        ? (result.condition, result.message ,result.value.email_verified) 
        : (result.condition, result.message, result.condition);
    }

    public (bool condition, string message, AuthRestModel infoResponse) GetUserInfo (
      string token)
    {
      AuthRestModel authResult;
      var url = $"{_applicationSetting.Authority}userinfo";
      bool requestResult = _cache.TryGetValue (token, out authResult);
      if (!requestResult)
      {
        var httpResult = _httpService.Get (url, token);
        if (!httpResult.condition)
        {
          _logService.Log (Minutz.Models.LogLevel.Exception, $"Auth0Repository.GetUserInfo -> there was a issue getting the details from auth0");
          throw new Exception ("Auth0 Exception");
        }
        authResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthRestModel> (httpResult.result);
        requestResult = httpResult.condition;
        // Set cache options.
        var cacheEntryOptions = new MemoryCacheEntryOptions ()
          // Keep in cache for this time, reset time if accessed.
          .SetSlidingExpiration (TimeSpan.FromMinutes (10));
        // Save data in cache.
        _cache.Set (token, authResult, cacheEntryOptions);
      }
      return (requestResult, "Success", authResult);
    }

    public (bool condition, string message, UserResponseModel tokenResponse) CreateToken (
      string username, string password)
    {
      if (string.IsNullOrEmpty (username))
      {
        return (false, _validationMessage, null);
      }
      if (string.IsNullOrEmpty (password))
      {
        return (false, _validationMessage, null);
      }
      _logService.Log (Minutz.Models.LogLevel.Info, $"username: {username} - password:{password}");

      var requestBody = new UserTokenRequestModel
      {
        grant_type = "password",
          username = username,
          password = password,
          client_id = _applicationSetting.ClientId,
          client_secret = _applicationSetting.ClientSecret,
          connection = _applicationSetting.AuthorityConnection
      }.ToJSON ();
      
      _logService.Log (Minutz.Models.LogLevel.Info, requestBody.ToString ());
      var tokenRequestResult = this._httpService.Post ($"{_applicationSetting.Authority}oauth/token", requestBody.ToStringContent ());
      _logService.Log (Minutz.Models.LogLevel.Info, tokenRequestResult.result);
      
      if (tokenRequestResult.condition)
      {
        var token = Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseModel> (tokenRequestResult.result);
        token.expires_in = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");
        return (tokenRequestResult.condition,
          "Success", token
        );
      }
      return (tokenRequestResult.condition, tokenRequestResult.result, null);
    }

    internal int JavascriptTime ()
    {
      return (int) DateTime.UtcNow
        .AddDays(1).Minute;
    }
  }
}