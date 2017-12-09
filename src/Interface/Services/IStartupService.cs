using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IStartupService
  {
    bool SendInvitationMessage(MeetingAttendee attendee,
                                Minutz.Models.ViewModels.MeetingViewModel meeting);
  }
}