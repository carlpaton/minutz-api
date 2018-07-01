using Minutz.Models.Message;
using System;

namespace Interface.Repositories.Feature.Dashboard
{
    public interface IUserMeetingsRepository
    {
        MeetingMessage Meetings(string email, string schema, string connectionString);

        MeetingMessage Meeting(Guid meetingId, string schema, string connectionString);

        MeetingMessage CreateEmptyUserMeeting (string email ,string schema, string connectionString);
    }
}