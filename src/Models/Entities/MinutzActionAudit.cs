using System;

namespace Minutz.Models.Entities
{
    public class MinutzActionAudit
    {
        public string Id { get; set; }
        public string ReferanceId { get; set; }
        public string ActionText { get; set; }
        public string PersonId { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public string Type { get; set; }
        public string Action { get; set; }
        public string ChangedBy { get; set; }
        public DateTime AuditDate { get; set; }
    }
}