using System;
using Interface.Helper;
using Interface.Services.Feature.Invite;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Minutz.Models.ViewModels;

namespace Core.Feature.Invite
{
  public class InvitationService: IInvitationService
  {
    private readonly IEmailValidationService _emailValidationService;
    
    public InvitationService(IEmailValidationService emailValidationService)
    {
      _emailValidationService = emailValidationService;
    }

    /// <summary>
    /// Invite a attendee
    /// Validate the attendee is new, alternatively update reference to the new instance if required
    /// </summary>
    /// <param name="invitee" typeof="MeetingAttendee">The new attendee</param>
    /// <param name="meeting" typeof="Minutz.Models.Entities.Meeting">The meeting entity for the invitation</param>
    /// <returns typeof="MessageBase">Result if was successful</returns>
    public MessageBase InviteUser(MeetingAttendee invitee, Minutz.Models.Entities.Meeting meeting)
    {
      var validEmail = _emailValidationService.Valid(invitee.Email);
      if (!validEmail) return new MessageBase{Condition = false, Message = $"{invitee.Email} is a invalid email address."};
      if(meeting.Id == Guid.Empty || string.IsNullOrEmpty(meeting.Name)) return new MessageBase { Condition = false, Message = "Meeting is invalid."};
      return new MessageBase { Condition = true, Message = "Successful"};
    }

    public bool SendMeetingInvitation(MeetingAttendee attendee, MeetingViewModel meeting, string instanceId)
    {
      throw new System.NotImplementedException();
    }
  }
}


