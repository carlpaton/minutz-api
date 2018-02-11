using System;
using AuthenticationRepository.Extensions;
using AuthenticationRepository.Models;
using AuthRepo.Models;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using System.Net.Http;
using System.Text;

namespace AuthenticationRepository
{
  public class Auth0Repository : IAuth0Repository
  {
    internal string _urlSignUp = "https://minutz.eu.auth0.com/dbconnections/signup";
    internal string _urlToken = "https://minutz.eu.auth0.com/oauth/token";
    internal string _clientId = Environment.GetEnvironmentVariable ("CLIENTID");
    internal string _domain = Environment.GetEnvironmentVariable ("DOMAIN");
    internal string _clientSecret = Environment.GetEnvironmentVariable ("CLIENTSECRET");
    internal string _connection = Environment.GetEnvironmentVariable ("CONNECTION");
    internal string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";
    private readonly IHttpService _httpService;

    public Auth0Repository ()
    {
      this._httpService = new HttpService ();
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

    public (bool condition, string message, string token) CreateToken (
      string username, string password)
    {
      if (string.IsNullOrEmpty (username))
      {
        return (false, this._validationMessage, string.Empty);
      }
      if (string.IsNullOrEmpty (password))
      {
        return (false, this._validationMessage, string.Empty);
      }
      var requestBody = new UserTokenRequestModel
      {
        grant_type = "password",
          username = username,
          password = password,
          client_id = "WDzuh9escySpPeAF5V0t2HdC3Lmo68a-",
          client_secret = "_kVUASQWVawA2pwYry-xP53kQpOALkEj_IGLWCSspXkpUFRtE_W-Gg74phrxZkz8",
          connection = "Username-Password-Authentication"
      }.ToJSON ();
      var conx =  requestBody.ToStringContent ();
      var tokenRequestResult = this._httpService.Post (this._urlToken, conx);
      if (tokenRequestResult.condition)
      {
        return (tokenRequestResult.condition, "Success", tokenRequestResult.result);
      }
      return (tokenRequestResult.condition, tokenRequestResult.result, string.Empty);
    }
  }
}