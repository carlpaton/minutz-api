using Minutz.Models.Message;

namespace Interface.Validation
{
    public interface IUserInviteValidationService
    {
        MessageBase UserHasBeenInvitedToMeeting(string meetingId, string email);
    }
}