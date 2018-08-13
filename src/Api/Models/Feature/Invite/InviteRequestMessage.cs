using System;
using System.Collections.Generic;

namespace Api.Models.Feature.Invite
{
    public class InviteRequestMessage
    {
        public Guid MeetingId { get; set; }
        public List<string> customRecipients { get; set; }
        public string message { get; set; }
        public bool includeAgenda { get; set; }
        public bool includeAttachments { get; set; }
    }

    public enum InviteAttendees
    {
        AllAttendess = 1,
        NewAttendees = 2,
        Custom = 3
    }
}