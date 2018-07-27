using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Status
{
    public interface IMeetingStatusRepository
    {
        MessageBase UpdateMeetingStatus(Guid meetingId, string status, string schema, string connectionString);
    }
}