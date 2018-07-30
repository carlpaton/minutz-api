using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Attachment
{
    public interface IMinutzMeetingAttachmentService
    {
        AttachmentMessage Get(Guid meetingId, AuthRestModel user);
        
        AttachmentMessage Add(Guid meetingId, string fileUrl, int order, AuthRestModel user);

        MessageBase Update(Guid meetingId, string fileUrl, int order, AuthRestModel user);
    }
}