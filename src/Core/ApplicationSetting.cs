using Interface.Services;
using System;

namespace Core
{
  public class ApplicationSetting: IApplicationSetting
  {
    public string Server => Environment.GetEnvironmentVariable("SERVER_ADDRESS");

    public string Catalogue => Environment.GetEnvironmentVariable("DEFAULT_CATALOGUE");

    public string Schema => Environment.GetEnvironmentVariable("DEFAULT_SCHEMA");

    public string Username => Environment.GetEnvironmentVariable("DEFAULT_USER");

    public string Password => Environment.GetEnvironmentVariable("DEFAULT_PASSWORD");

    public string Authority => Environment.GetEnvironmentVariable("AUTHORITY");

    public string CreateConnectionString(string server, 
                                         string catalogue, 
                                         string username, 
                                         string password)
    {
      return $"Server={server};User ID={username};pwd={password};database={catalogue};";
    }
  }
}
