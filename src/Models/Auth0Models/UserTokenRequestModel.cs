using System;
namespace Models.Auth0Models
{
  public class UserTokenRequestModel
  {
    public string grant_type { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string connection { get; set; }
  }
}