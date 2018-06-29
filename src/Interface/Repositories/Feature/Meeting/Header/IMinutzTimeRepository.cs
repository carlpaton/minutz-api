using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMinutzTimeRepository
    {
        MessageBase Update(string meetingId, string time, string schema, string connectionString);
    }
}