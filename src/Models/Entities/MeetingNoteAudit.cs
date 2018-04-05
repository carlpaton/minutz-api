using System;

namespace Minutz.Models.Entities
{
    public class MeetingNoteAudit
    {
        public string Id { get; set; }
        public string ReferanceId { get; set; }
        public string NoteText { get; set; }
        public string MeetingAttendeeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Action { get; set; }
        public string ChangedBy { get; set; }
        public DateTime AuditDate { get; set; }
    }
}