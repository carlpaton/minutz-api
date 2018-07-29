using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Attachment
{
    public interface IMinutzMeetingAttachmentService
    {
        MessageBase Add(string meetingId, string fileUrl, AuthRestModel user);
    }
}