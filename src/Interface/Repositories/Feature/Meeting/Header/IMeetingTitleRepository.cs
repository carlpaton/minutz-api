using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingTitleRepository
    {
        MessageBase Update(string meetingId, string title, string schema, string connectionString);
    }
}