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
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly IApplicationSetting _applicationSetting;
    private readonly IAuth0Repository _auth0Repository;
    private readonly IUserRepository _userRepository;
    private readonly ILogService _logService;
    private IMemoryCache _cache;
    public AuthenticationService (
      IApplicationSetting applicationSetting, IMemoryCache memoryCache, IAuth0Repository auth0Repository, ILogService logService, IUserRepository userRepository, IApplicationSetupRepository applicationSetupRepository)
    {
      this._applicationSetting = applicationSetting;
      this._cache = memoryCache;
      this._auth0Repository = auth0Repository;
      this._logService = logService;
      this._userRepository = userRepository;
      this._applicationSetupRepository = applicationSetupRepository;
    }

    public (bool condition, string message, UserResponseModel tokenResponse) Login (
      string email, string password)
    {
      return this._auth0Repository.CreateToken (email, password);
    }

    public (bool condition, string message, AuthRestModel tokenResponse) CreateUser (
      string name, string username, string email, string password)
    {
      // first check if user is not in the db;

      // create the user in auth0;
      var instanceId = Guid.NewGuid ().ToString ();
      (bool condition, string message, AuthRestModel tokenResponse) auth0Response =
        this._auth0Repository.CreateUser (name, username, email, password, "User", $"A_{instanceId}");
      if (auth0Response.condition)
      {
        this._logService.Log (Minutz.Models.LogLevel.Info, $"Auth0 created user: {username}.");
        // create the user in the db;
        var db_user = this._userRepository.CreateNewUser (auth0Response.tokenResponse,
          this._applicationSetting.Schema, _applicationSetting.CreateConnectionString ());

        // create the schema as the user is trial user;
        (string userConnectionString, string masterConnectionString) connectionStrings = this.GetConnectionStrings ();
        var schemaCreateResult = _userRepository.CreateNewSchema (
          auth0Response.tokenResponse, connectionStrings.userConnectionString, connectionStrings.masterConnectionString);
        
        // create the tables as the user is trial user;
        var tablesCreateResult =  this._applicationSetupRepository.CreateSchemaTables (
          _applicationSetting.Schema, schemaCreateResult, _applicationSetting.CreateConnectionString ());

      }
      return auth0Response;
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

    internal (string userConnectionString, string masterConnectionString) GetConnectionStrings ()
    {
      var masterConnectionString = _applicationSetting.CreateConnectionString (
        _applicationSetting.Server,
        "master",
        _applicationSetting.Username,
        _applicationSetting.Password);
      var userConnectionString = _applicationSetting.CreateConnectionString (
        _applicationSetting.Server,
        _applicationSetting.Catalogue,
        _applicationSetting.Username,
        _applicationSetting.Password);
      return (userConnectionString, masterConnectionString);
    }
  }
}