using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting
{
    public interface IMinutzAttendeeService
    {
        AttendeeMessage GetAttendees(Guid meetingId, AuthRestModel user);
        
        AttendeeMessage AddAttendee(Guid meetingId, MeetingAttendee attendee, AuthRestModel user);

        AttendeeMessage UpdateAttendee(Guid meetingId, MeetingAttendee attendee, AuthRestModel user);

        MessageBase DeleteAttendee(Guid meetingId, string attendeeEmail, AuthRestModel user);
    }
}