using System;

namespace Minutz.Models.Entities
{
    public class AvailibleAttendeeAudit
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string PersonIdentity { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Action { get; set; }
        public string ChangedBy { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
