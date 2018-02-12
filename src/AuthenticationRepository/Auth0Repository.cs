using System;
using AuthenticationRepository.Extensions;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Models;
using Models.Auth0Models;

namespace AuthenticationRepository
{
  public class Auth0Repository : IAuth0Repository
  {
    internal string _urlSignUp = $"https://{Environment.GetEnvironmentVariable ("DOMAIN")}/dbconnections/signup";
    internal string _urlToken = $"https://{Environment.GetEnvironmentVariable ("DOMAIN")}/oauth/token";
    internal string _clientId = Environment.GetEnvironmentVariable ("CLIENTID");
    internal string _domain = Environment.GetEnvironmentVariable ("DOMAIN");
    internal string _clientSecret = Environment.GetEnvironmentVariable ("CLIENTSECRET");
    internal string _connection = Environment.GetEnvironmentVariable ("CONNECTION");
    internal string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";
    private readonly IHttpService _httpService;
    private readonly ILogService _logService;
    public Auth0Repository (
      ILogService logService)
    {
      this._httpService = new HttpService ();
      this._logService = logService;
    }
    public AuthRestModel CreateUser (
      string name, string email, string password, string role, string instanceId)
    {
      var requestBody = new UserRequestModel
        {
          client_id = _clientId, email = email,
            password = password,
            connection = _connection
        }.Prepare (instanceId, name, role)
        .ToJSON ().ToStringContent ();
      var createResult = this._httpService.Post (this._urlSignUp, requestBody);
      if (!createResult.condition)
      {
        throw new Exception (createResult.result);
      }
      var result = new AuthRestModel
      {
        Email = email,
        Nickname = name,
        InstanceId = instanceId,
        Role = role
      };
      return result;
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
      this._logService.Log(Minutz.Models.LogLevel.Info, $"username: {username} - password:{password}");
      var requestBody = new UserTokenRequestModel
      {
        grant_type = "password",
        username = username,
        password = password,
        client_id = this._clientId,
        client_secret = this._clientSecret,
        connection = this._connection
      }.ToJSON ();
      this._logService.Log(Minutz.Models.LogLevel.Info, requestBody.ToString());
      var tokenRequestResult = this._httpService.Post (this._urlToken, requestBody.ToStringContent ());
      this._logService.Log(Minutz.Models.LogLevel.Info, tokenRequestResult.result);
      if (tokenRequestResult.condition)
      {
        return (tokenRequestResult.condition,
                "Success",
                Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseModel>(tokenRequestResult.result));
      }
      return (tokenRequestResult.condition, tokenRequestResult.result, null);
    }
  }
}