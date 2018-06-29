using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMinutzTimeService
    {
        MessageBase Update(string meetingId, string time, AuthRestModel user);
    }
}