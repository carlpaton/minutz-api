using System;

namespace Minutz.Models.Entities
{
    public class MeetingAgendaAudit
    {
        public Guid Id { get; set; }
        public string ReferenceId { get; set; }
        public string AgendaHeading { get; set; }
        public string AgendaText { get; set; }
        public string MeetingAttendeeId { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsComplete { get; set; }
        public string Action { get; set; }
        public string ChangedBy { get; set; }
        public DateTime AuditDate { get; set; }
    }
}