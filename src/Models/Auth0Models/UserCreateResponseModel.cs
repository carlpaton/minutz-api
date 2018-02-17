using Models.Auth0Models;

namespace Models
{
    public class UserCreateResponseModel
    {
        public string _id { get; set; }
        public bool email_verified { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public UserMetadata user_metadata { get; set; }
    }
}