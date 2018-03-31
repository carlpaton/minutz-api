using System;

namespace Minutz.Models.Auth0Models
{
    public class UserQueryModel
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string username { get; set; }
        public string picture { get; set; }
        public bool email_verified { get; set; }
        public string email { get; set; }
        public DateTime updated_at { get; set; }
        public UserQueryMetadata user_metadata { get; set; }
    }
}
