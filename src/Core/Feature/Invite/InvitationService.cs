using Interface.Helper;
using Minutz.Models.Entities;
namespace Core.Feature.Invite
{
  public class InvitationService
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
    /// <returns></returns>
    public bool InviteUser(MeetingAttendee invitee, Minutz.Models.Entities.Meeting meeting)
    {
      if (string.IsNullOrEmpty(invitee.Email)) return false;
      return true;
    }
  }
}


