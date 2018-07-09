using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting
{
    public interface IMinutzAttendeeRepository
    {
        AttendeeMessage GetAttendees(Guid meetingId, string schema, string connectionString, string masterConnectionString);
        
        AttendeeMessage AddAttendee(Guid meetingId, MeetingAttendee attendee, string schema, string connectionString);

        AttendeeMessage UpdateAttendee(Guid meetingId, MeetingAttendee attendee, string schema,
                                       string connectionString);
        
        MessageBase DeleteAttendee(Guid meetingId, string attendeeEmail, string schema, string connectionString);
    }
}