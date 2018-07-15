using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IMinutzAvailabilityRepository
    {
        AttendeeMessage GetAvailableAttendees(string schema, string connectionString, string masterConnectionString);

        AttendeeMessage CreateAvailableAttendee(MeetingAttendee attendee, string schema, string connectionString);
    }
}