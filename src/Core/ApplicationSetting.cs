using Interface.Services;
using System;

namespace Core
{
  public class ApplicationSetting: IApplicationSetting
  {
    public string Catalogue { get { return Environment.GetEnvironmentVariable("APPLICATION_CATALOGUE"); } }

    public string Schema { get { return Environment.GetEnvironmentVariable("APPLICATION_SCHEMA"); } }

    public string Username { get { return Environment.GetEnvironmentVariable("ÄPPLICATION_USERNAME"); } }

    public string Password { get { return Environment.GetEnvironmentVariable("APPLICATION_PASSWORD"); } }
  }
}
