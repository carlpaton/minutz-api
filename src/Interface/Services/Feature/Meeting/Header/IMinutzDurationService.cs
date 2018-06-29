using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMinutzDurationService
    {
        MessageBase Update(string meetingId, int duration, AuthRestModel user);
    }
}