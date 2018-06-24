using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingDurationRepository
    {
        MessageBase Update(string meetingId, int duration, string schema, string connectionString);
    }
}