using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMinutzLocationService
    {
        MessageBase Update(string meetingId, string location, AuthRestModel user);
    }
}