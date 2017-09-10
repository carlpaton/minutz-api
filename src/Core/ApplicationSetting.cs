using Interface.Services;
using System;

namespace Core
{
  public class ApplicationSetting: IApplicationSetting
  {
    public string Server { get { return Environment.GetEnvironmentVariable("SERVER_ADDRESS"); } }

    public string Catalogue { get { return Environment.GetEnvironmentVariable("DEFAULT_CATALOGUE"); } }

    public string Schema { get { return Environment.GetEnvironmentVariable("DEFAULT_SCHEMA"); } }

    public string Username { get { return Environment.GetEnvironmentVariable("DEFAULT_USER"); } }

    public string Password { get { return Environment.GetEnvironmentVariable("DEFAULT_PASSWORD"); } }

    public string Authority { get { return Environment.GetEnvironmentVariable("AUTHORITY"); } }

    public string CreateConnectionString(string server, 
                                         string catalogue, 
                                         string username, 
                                         string password)
    {
      return $"Server={server};User ID={username};pwd={password};database={catalogue};";
    }
  }
}
