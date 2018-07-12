using System;
using Api.Models.Feature.Invite;
using Minutz.Models.Entities;

namespace Api.Extensions
{
    /// <summary>
    /// Check the object that is used for the Invite Attendee
    /// </summary>
    public static class InviteRequestExtensions
    {
        public static InviteValidateResult Validate(this MeetingAttendee model)
        {
            if (model.ReferenceId == Guid.Empty)
            {
               return new InviteValidateResult{ Message = "Please provide a valid meetingId", Condition = false};
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                return new InviteValidateResult { Message = "Please provide a valid email address", Condition = false};
            }
            return string.IsNullOrEmpty(model.Name) 
                ? new InviteValidateResult{ Message = "Please provide a valid name.", Condition = false} 
                : new InviteValidateResult {Condition = true, Message = "Valid"};
        }
    }
}