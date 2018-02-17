using System;
using AuthenticationRepository.Extensions;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Minutz.Models.Entities;
using Models;
using Models.Auth0Models;
using System.Net.Http;
using System.Text;

namespace AuthenticationRepository
{
  public class Auth0Repository : IAuth0Repository
  {
    internal string _urlSignUp = $"https://{Environment.GetEnvironmentVariable ("DOMAIN")}/dbconnections/signup";
    internal string _urlToken = $"https://{Environment.GetEnvironmentVariable ("DOMAIN")}/oauth/token";
    internal string _urlInfo = $"https://{Environment.GetEnvironmentVariable ("DOMAIN")}/userinfo";
    internal string _clientId = Environment.GetEnvironmentVariable ("CLIENTID");
    internal string _domain = Environment.GetEnvironmentVariable ("DOMAIN");
    internal string _clientSecret = Environment.GetEnvironmentVariable ("CLIENTSECRET");
    internal string _connection = Environment.GetEnvironmentVariable ("CONNECTION");
    internal string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";
    private readonly IHttpService _httpService;
    private readonly ILogService _logService;
    private IMemoryCache _cache;
    public Auth0Repository (
      ILogService logService, IMemoryCache memoryCache)
    {
      this._httpService = new HttpService ();
      this._logService = logService;
      this._cache = memoryCache;
    }
    public (bool condition, string message, AuthRestModel value) CreateUser (
      string name, string username, string email, string password, string role, string instanceId)
    {
      var requestBody = new UserRequestModel
        {
          client_id = _clientId,
            email = email,
            username = username,
            password = password,
            connection = _connection
        }.Prepare (instanceId, name, role)
        .ToJSON ().ToStringContent ();
      var createResult = this._httpService.Post (this._urlSignUp, requestBody);
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
        (bool condition, string message, AuthRestModel infoResponse) userInfoResult = this.GetUserInfo (tokenResult.tokenResponse.id_token);
        if(userInfoResult.condition)
        {
          result.Sub = userInfoResult.infoResponse.Sub;
          result.Picture = userInfoResult.infoResponse.Picture;
        }
      }

      return (createResult.condition, createResult.result, result);
    }

    public void CheckIfUserIsValidated ()
    {

    }

    public void ValidateUser ()
    {
      //https://{{auth0_domain}}/oauth/token
      //grant_type = password
      //client_id=
      //client_secret=
      //username
      //password
      //connection=Username-Password-Authentication

    }

    public (bool condition, string message, AuthRestModel infoResponse) GetUserInfo (
      string token)
    {
      AuthRestModel authResult;
      bool requestResult = _cache.TryGetValue (token, out authResult);
      if (!requestResult)
      {
        var httpResult = this._httpService.Get (this._urlInfo, token);
        if (!httpResult.condition)
        {
          this._logService.Log (Minutz.Models.LogLevel.Exception, $"Auth0Repository.GetUserInfo -> there was a issue getting the details from auth0");
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
      return (requestResult,"Success",authResult);
    }

    public (bool condition, string message, UserResponseModel tokenResponse) CreateToken (
      string username, string password)
    {
      if (string.IsNullOrEmpty (username))
      {
        return (false, this._validationMessage, null);
      }
      if (string.IsNullOrEmpty (password))
      {
        return (false, this._validationMessage, null);
      }
      this._logService.Log (Minutz.Models.LogLevel.Info, $"username: {username} - password:{password}");
      var requestBody = new UserTokenRequestModel
      {
        grant_type = "password",
          username = username,
          password = password,
          client_id = this._clientId,
          client_secret = this._clientSecret,
          connection = this._connection
      }.ToJSON ();
      this._logService.Log (Minutz.Models.LogLevel.Info, requestBody.ToString ());
      var tokenRequestResult = this._httpService.Post (this._urlToken, requestBody.ToStringContent ());
      this._logService.Log (Minutz.Models.LogLevel.Info, tokenRequestResult.result);
      if (tokenRequestResult.condition)
      {
        return (tokenRequestResult.condition,
          "Success",
          Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseModel> (tokenRequestResult.result));
      }
      return (tokenRequestResult.condition, tokenRequestResult.result, null);
    }
  }
}