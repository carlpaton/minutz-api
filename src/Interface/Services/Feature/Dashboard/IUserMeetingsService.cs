using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Dashboard
{
    public interface IUserMeetingsService
    {
        MeetingMessage Meetings(AuthRestModel user);
    }
}