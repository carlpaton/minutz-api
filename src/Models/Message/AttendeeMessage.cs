using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class AttendeeMessage: MessageBase
    {
        public MeetingAttendee Attendee { get; set; }
        public List<MeetingAttendee> Attendees { get; set; }
    }
}