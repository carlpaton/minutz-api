using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IUserManageMeetingRepository
    {
        MeetingMessage Update(Minutz.Models.Entities.Meeting meeting, string schema, string connectionString);
    }
}