using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Invite
 {
     public interface IInvitationService
     {
         MessageBase InviteUser(AuthRestModel user, MeetingAttendee invitee, Minutz.Models.Entities.Meeting meeting);
         
         bool SendMeetingInvitation(Minutz.Models.Entities.MeetingAttendee attendee,
                                    Minutz.Models.ViewModels.MeetingViewModel meeting,
                                    string instanceId);
     }
 }