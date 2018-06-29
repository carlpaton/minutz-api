using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMinutzDurationRepository
    {
        MessageBase Update(string meetingId, int duration, string schema, string connectionString);
    }
}