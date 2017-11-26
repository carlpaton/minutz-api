using Models.Entities;

namespace Interface.Services
{
    public interface IStartupService
    {
          bool SendInvitationMessage (MeetingAttendee attendee);
    }
}