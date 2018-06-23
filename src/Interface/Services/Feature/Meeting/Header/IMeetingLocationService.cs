using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMeetingLocationService
    {
        MessageBase Update(string meetingId, string location, AuthRestModel user);
    }
}