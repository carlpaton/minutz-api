using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Attachment
{
    public interface IMinutzMeetingAttachmentRepository
    {
        AttachmentMessage Get(Guid meetingId, string schema, string connectionString);
        
        AttachmentMessage Add(Guid meetingId, string fileName, int order, string schema, string connectionString);

        MessageBase Update(Guid meetingId, string fileName, int order, string schema, string connectionString);
    }
}