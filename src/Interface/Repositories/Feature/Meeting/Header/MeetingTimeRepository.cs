using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingTimeRepository
    {
        MessageBase Update(string meetingId, string time, string schema, string connectionString);
    }
}