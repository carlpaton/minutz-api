using Minutz.Models.Auth0Models;

namespace Minutz.Models.Message
{
    public class AuthUserQueryResponse: MessageBase
    {
        public UserQueryModel User { get; set; }
    }
}