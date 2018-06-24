using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingTagRepository
    {
        MessageBase Update(string meetingId, string tags, string schema, string connectionString);
    }
}