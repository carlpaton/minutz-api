using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting
{
    public interface IMinutzAvailabilityService
    {
        AttendeeMessage GetAvailableAttendees(AuthRestModel user);
    }
}