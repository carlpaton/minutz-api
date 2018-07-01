using Minutz.Models.Entities;
using Minutz.Models.Message;
using System;

namespace Interface.Services.Feature.Dashboard
{
    public interface IUserMeetingsService
    {
        MeetingMessage Meetings(AuthRestModel user);

        MeetingMessage Meeting(AuthRestModel user, Guid meetingId);

        MeetingMessage CreateEmptyUserMeeting (AuthRestModel user);
    }
}