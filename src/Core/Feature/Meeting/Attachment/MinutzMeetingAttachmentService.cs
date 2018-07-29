using Interface.Services.Feature.Meeting.Attachment;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Attachment
{
    public class MinutzMeetingAttachmentService: IMinutzMeetingAttachmentService
    {
        public MessageBase Add(string meetingId, string fileUrl, AuthRestModel user)
        {
            throw new System.NotImplementedException();
        }
    }
}