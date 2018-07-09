using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IMinutzAvailabilityRepository
    {
        AttendeeMessage GetAvailableAttendees(string schema, string connectionString);
    }
}