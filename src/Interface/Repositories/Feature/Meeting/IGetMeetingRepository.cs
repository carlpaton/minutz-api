using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IGetMeetingRepository
    {
        MeetingMessage Get(Guid meetingId, string schema, string connectionString);
    }
}