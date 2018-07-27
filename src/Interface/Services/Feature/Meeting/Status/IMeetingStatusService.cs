using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Status
{
    public interface IMeetingStatusService
    {
        MessageBase UpdateMeetingStatus(Guid meetingId, string status, AuthRestModel user);
    }
}