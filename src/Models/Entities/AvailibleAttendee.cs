using System;

namespace Minutz.Models.Entities
{
    public class AvailibleAttendee
    {
        public Guid Id { get; set; }
        public Guid ReferanceId { get; set; }
        public string PersonIdentity { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Roles { get; set; }
    }
}