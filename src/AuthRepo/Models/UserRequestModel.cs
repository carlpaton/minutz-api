using System;
namespace AuthenticationRepository.Models
{
  public class UserRequestModel
  {
    public string client_id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string connection { get; set; }
    public UserMetadata user_metadata { get; set; }
    public AppMetadata app_metadata { get; set; }
  }
  public class UserMetadata
  {
    public string name { get; set; }
    public string role { get; set; }
    public string instance { get; set; }
  }
  public class AppMetadata
  {
    public string related { get; set; }
  }
}
