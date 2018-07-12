using System;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting
{
    public interface IGetMeetingService
    {
        MeetingMessage GetMeeting(string instanceId, Guid meetingId);
    }
}