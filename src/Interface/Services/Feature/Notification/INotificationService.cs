using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Notification
{
    public interface INotificationService
    {
        MessageBase SendMeetingInvitation(MeetingAttendee attendee, Minutz.Models.Entities.Meeting meeting, string instanceId);

        MessageBase SendMeetingMinutes(MeetingAttendee attendee, Minutz.Models.Entities.Meeting meeting, string instanceId);
    }
}