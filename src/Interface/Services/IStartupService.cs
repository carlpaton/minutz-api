using Models.Entities;

namespace Interface.Services
{
    public interface IStartupService
    {
          bool SendSimpleMessage (MeetingAttendee attendee);
    }
}