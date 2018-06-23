using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingLocationRepository
    {
        MessageBase Update(string meetingId, string location, string schema, string connectionString);
    }
}