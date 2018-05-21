using System;
using System.Collections.Generic;
using System.Linq;
using AuthenticationRepository.Extensions;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Minutz.Models;
using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
using Minutz.Models.Message;
using Models.Auth0Models;
using Newtonsoft.Json;

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
      string name, string email, string password, string role, string instanceId)
    {
      var requestBody = new UserRequestModel
        {
          client_id = _applicationSetting.ClientId,
            email = email,
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
      
      var tokenResult = this.CreateToken (email, password);
      
      if (tokenResult.Condition)
      {
        var userInfoResult = this.GetUserInfo (tokenResult.AuthTokenResponse.access_token);
        if (userInfoResult.Condition)
        {
          result.Sub = userInfoResult.InfoResponse.Sub;
          result.Picture = userInfoResult.InfoResponse.Picture;
        }
      }

      return (createResult.condition, createResult.result, result);
    }

    public AuthUserQueryResponse SearchUserByEmail
      (string email)
    {
      var result = new AuthUserQueryResponse { Condition = false, Message = string.Empty, User = new UserQueryModel()};
      //var emailEncoded = System.Net.WebUtility.UrlEncode(email);
      var url = $"{_applicationSetting.Authority}api/v2/users-by-email?email={email}";
      var tokenResult = GetManagementApiToken();
      
      var httpResult = _httpService.Get (url, tokenResult.token );
      if (!httpResult.condition)
      {
        _logService.Log (LogLevel.Exception, $"(bool condition, string message, UserQueryModel value)  SearchUserByEmail -> there was a issue getting the details from auth0");
        result.Message = "There was a issue getting the user information.";
        return result;
      }
      try
      {
        var deserializedResult = JsonConvert.DeserializeObject<List<UserQueryModel>> (httpResult.result);
        result.Condition = true;
        result.User = deserializedResult.First();
        return result;
      }
      catch (Exception e)
      {
        _logService.Log(LogLevel.Exception, e.InnerException.Message);
        result.Message = e.InnerException.Message;
        return result;
      }
    }

    public (bool condition, string message, bool value) ValidateUser 
      (string email)
    {
      var result = SearchUserByEmail(email);
      return result.Condition 
        ? (result.Condition, result.Message ,result.User.email_verified) 
        : (result.Condition, result.Message, result.Condition);
    }

    public AuthRestModelResponse GetUserInfo 
      (string token)
    {
      var result = new AuthRestModelResponse { Condition = false, Message = string.Empty, InfoResponse = new AuthRestModel()};
      AuthRestModel authResult;
      var url = $"{_applicationSetting.Authority}userinfo";
      bool requestResult = _cache.TryGetValue (token, out authResult);
      if (!requestResult)
      {
        var httpResult = _httpService.Get (url, token);
        if (!httpResult.condition)
        {
          _logService.Log (LogLevel.Exception, $"Auth0Repository.GetUserInfo -> there was a issue getting the details from auth0");
          result.Message = httpResult.result;
          return result;
        }
        authResult = JsonConvert.DeserializeObject<AuthRestModel> (httpResult.result);
        result.Condition = httpResult.condition;
        var cacheEntryOptions = new MemoryCacheEntryOptions ().SetSlidingExpiration (TimeSpan.FromMinutes (10));
        _cache.Set (token, authResult, cacheEntryOptions);
      }
      else
      {
        result.Condition = true;
      }
      result.InfoResponse = authResult;
      return result;
    }

    public TokenResponse CreateToken 
      (string email, string password)
    {
      var result = new TokenResponse { Condition = false, Message = string.Empty, AuthTokenResponse = new UserResponseModel()};
      if (string.IsNullOrEmpty (email))
      {
        result.Message = _validationMessage;
        return result;
      }
      if (string.IsNullOrEmpty (password))
      {
        result.Message = _validationMessage;
        return result;
      }
      _logService.Log (LogLevel.Info, $"username: {email} - password:{password}");

      var requestBody = new UserTokenRequestModel
      {
        grant_type = "password",
          username = email,
          password = password,
          client_id = _applicationSetting.ClientId,
          client_secret = _applicationSetting.ClientSecret,
          connection = _applicationSetting.AuthorityConnection
      }.ToJSON ();
      
       _logService.Log (LogLevel.Info, requestBody);
      var tokenRequestResult = _httpService.Post ($"{_applicationSetting.Authority}oauth/token", requestBody.ToStringContent ());

      _logService.Log (LogLevel.Info, tokenRequestResult.result);
      if (tokenRequestResult.condition)
      {
        var token = JsonConvert.DeserializeObject<UserResponseModel> (tokenRequestResult.result);
        token.expires_in = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");
        result.Condition = true;
        result.AuthTokenResponse = token;
        return result;
      }
      result.Message = tokenRequestResult.result;
      return result;
    }

    private (bool condition, string message, string token) GetManagementApiToken()
    {
      var payload = new ManagmentApiTokenRequestModel
      {
        grant_type = "client_credentials",
        client_id = _applicationSetting.AuthorityManagementClientId,
        client_secret = _applicationSetting.AuthorityManagementClientSecret,
        audience = $"{_applicationSetting.Authority}api/v2/"
      }.ToJSON().StringContent();

      var tokenRequestResult =
        _httpService.Post($"{_applicationSetting.Authority}oauth/token", payload);
      _logService.Log(LogLevel.Info, tokenRequestResult.result);

      if (!tokenRequestResult.condition)
      {
        return (tokenRequestResult.condition, "Issue getting token for management api", string.Empty);
      }

      var response = JsonConvert.DeserializeObject<UserResponseModel>(tokenRequestResult.result);
      return (tokenRequestResult.condition, "Success", response.access_token);
    }

    internal int JavascriptTime ()
    {
      return (int) DateTime.UtcNow
        .AddDays(1).Minute;
    }
  }
}