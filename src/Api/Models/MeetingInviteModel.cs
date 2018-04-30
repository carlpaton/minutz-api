namespace Api.Models
{
    public class MeetingInviteModel
    {
        public string meetingId { get; set; }
        public InviteAttendees recipients { get; set; }
        public string[] customRecipients { get; set; }
        public string message { get; set; }
        public bool includeAgenda { get; set; }
        public bool includeAttachments { get; set; }
    }
}
