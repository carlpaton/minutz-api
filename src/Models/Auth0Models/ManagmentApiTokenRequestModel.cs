namespace Minutz.Models.Auth0Models
{
    public class ManagmentApiTokenRequestModel
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
    }
}