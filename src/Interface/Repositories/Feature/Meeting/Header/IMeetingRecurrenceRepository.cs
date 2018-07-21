using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingRecurrenceRepository
    {
        MessageBase Update(string meetingId, int recurrenceType, string schema, string connectionString);
    }
}