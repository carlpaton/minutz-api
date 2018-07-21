using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMinutzRecurrenceService
    {
        MessageBase Update(string meetingId, int recurrence, AuthRestModel user);
    }
}