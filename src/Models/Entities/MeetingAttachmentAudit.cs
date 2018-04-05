using System;

namespace Minutz.Models.Entities
{
    public class MeetingAttachmentAudit
    {
        public string Id { get; set; }
        public string ReferanceId { get; set; }
        public string FileName { get; set; }
        public string MeetingAttendeeId { get; set; }
        public DateTime Date { get; set; }
        public byte[] FileData { get; set; }
        public string Action { get; set; }
        public string ChangedBy { get; set; }
        public DateTime AuditDate { get; set; }
    }
}