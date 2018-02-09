using System;
using System.Net.Http;
using System.Text;
using AuthenticationRepository.Models;
using AuthRepo.Models;

namespace AuthenticationRepository
{
  public class Auth0Settings
  {
    internal const string _domain = "https://minutz.eu.auth0.com/api/v2/";
    internal const string _apiId = "5a7b573792663525d6d70b21";
    internal const string _clientappName = "minutz-web-app";
    internal const string _clientDmain = "minutz.eu.auth0.com";
    internal const string _clientId = "WDzuh9escySpPeAF5V0t2HdC3Lmo68a-";
    internal const string _api_clientId = "mMObcH709vRh6tOQraUoWC4Ox11uljvP";
    internal const string _clientSecret = "_kVUASQWVawA2pwYry-xP53kQpOALkEj_IGLWCSspXkpUFRtE_W-Gg74phrxZkz8";

    public void CreateUser()
    {
      var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(
        new UserRequestModel
        {
          client_id = _clientId,
          email = "info@docker.durban",
          password = "@nathan001",
          connection = "Username-Password-Authentication",
          user_metadata = new UserMetadata
          {
            instance = "instnaceId",
            name = "leeroya",
            role = "User"
          }
        });
      var content = new StringContent(jsonString,
                         Encoding.UTF8,
                         "application/json");
      var c = new HttpClient();
      c.DefaultRequestHeaders.Add("Accept", "application/json");

      var url = "https://minutz.eu.auth0.com/dbconnections/signup";
      var result = c.PostAsync(url, content).Result;
      var reason = result.Content.ReadAsStringAsync().Result;
      var q = result;
    }

    public void CheckIfUserIsValidated()
    {

    }

    public void ValidateUser()
    {
      //https://{{auth0_domain}}/oauth/token
//grant_type = password
//client_id=
//client_secret=
//username
//password
//connection=Username-Password-Authentication


    }

    public string Token()
    {
      var url = "https://minutz.eu.auth0.com/oauth/token";
      var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(
        new UserTokenRequestModel
        {
          grant_type = "password",
          username = "info@docker.durban",
          password = "@nathan001",
          client_id = "WDzuh9escySpPeAF5V0t2HdC3Lmo68a-",
          client_secret = "_kVUASQWVawA2pwYry-xP53kQpOALkEj_IGLWCSspXkpUFRtE_W-Gg74phrxZkz8"
        }));


      var c = new HttpClient();
      c.DefaultRequestHeaders.Add("Accept", "application/json");
      return c.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
    }

  }
}
