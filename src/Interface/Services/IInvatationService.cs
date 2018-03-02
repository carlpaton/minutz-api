using System;
namespace Interface.Services
{
  public interface IInvatationService
  {
    bool SendMeetingInvatation(Minutz.Models.Entities.MeetingAttendee attendee,
                               Minutz.Models.ViewModels.MeetingViewModel meeting,
                               string instanceId);
  }
}
