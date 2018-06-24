using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMeetingTimeService
    {
        MessageBase Update(string meetingId, string time, AuthRestModel user);
    }
}