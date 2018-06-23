using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMeetingTitleService
    {
        MessageBase Update(string meetingId, string title, AuthRestModel user);
    }
}