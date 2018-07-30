using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class AttachmentMessage: MessageBase
    {
        public List<MeetingAttachment> AttachmentCollection { get; set; }
        public MeetingAttachment Attachment { get; set; }
    }
}