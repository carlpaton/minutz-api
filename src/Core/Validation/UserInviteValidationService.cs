using Interface.Validation;
using Minutz.Models.Message;

namespace Core.Validation
{
    public class UserInviteValidationService : IUserInviteValidationService
    {
        public UserInviteValidationService()
        {
            
        }

        public MessageBase UserHasBeenInvitedToMeeting(string meetingId, string email)
        {
            return new MessageBase();
        }
    }
}