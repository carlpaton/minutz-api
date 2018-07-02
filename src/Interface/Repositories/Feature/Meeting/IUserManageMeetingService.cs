using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IUserManageMeetingService
    {
         MeetingMessage UpdateMeeting (Minutz.Models.Entities.Meeting meeting, AuthRestModel user);
    }
}