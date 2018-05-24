namespace Api.Models
{
    public class CreateUserModel
    {
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string RefInstanceId { get; set; }
        public string refMeetingId { get; set; }
        public string role { get; set; }
    }
}