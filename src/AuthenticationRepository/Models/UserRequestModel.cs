using System;
using AuthRepo.Models;

namespace AuthenticationRepository.Models
{
  public class UserRequestModel
  {
    public string client_id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string connection { get; set; }
    public UserMetadata user_metadata { get; set; }
  }
}