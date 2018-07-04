using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IMeetingObjectivePurposeRepository
    {
        MeetingMessage UpdateObjective(Guid meetingId, string objective, string schema, string connectionString);
        
        MeetingMessage UpdatePurpose(Guid meetingId, string objective, string schema, string connectionString);
    }
}