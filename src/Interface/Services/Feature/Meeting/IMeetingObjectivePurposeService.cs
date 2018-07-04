using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting
{
    public interface IMeetingObjectivePurposeService
    {
        MeetingMessage UpdateObjective(Guid meetingId,string objective, AuthRestModel user);

        MeetingMessage UpdatePurpose(Guid meetingId, string purpose, AuthRestModel user);
    }
}