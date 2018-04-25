using Models.Auth0Models;

namespace Minutz.Models.Message
{
    public class TokenResponse : MessageBase
    {
       public  UserResponseModel AuthTokenResponse { get; set; }
    }
}