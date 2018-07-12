namespace Interface.Services.Feature.Invite
 {
     public interface IInvitationService
     {
         bool SendMeetingInvitation(Minutz.Models.Entities.MeetingAttendee attendee,
                                    Minutz.Models.ViewModels.MeetingViewModel meeting,
                                    string instanceId);
     }
 }