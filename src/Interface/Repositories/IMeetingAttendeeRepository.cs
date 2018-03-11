using System;
using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories {
  public interface IMeetingAttendeeRepository {
    MeetingAttendee Get (
      Guid id, string schema, string connectionString);
    
    List<MeetingAttendee> GetMeetingAttendees (
      Guid referenceId, string schema, string connectionString, string masterConnectionString);
    
    List<MeetingAttendee> GetAvalibleAttendees (
      string schema, string connectionString);
    
    IEnumerable<MeetingAttendee> List (
      string schema, string connectionString);
    
    bool Add (
      MeetingAttendee action, string schema, string connectionString);
    
    bool AddInvitee (
      MeetingAttendee attendee,
      string schema,
      string connectionString,
      string defaultConnectionString,
      string defaultSchema,
      string referenceMeetingId,
      string inviteEmail);

    (bool condition, string message) UpdateInviteeStatus (
      string personIdentity, string newPersonIdentity, string status, string schema, string connectionString);
    
    bool Update (MeetingAttendee action, string schema, string connectionString);
    
    bool DeleteMeetingAttendees (Guid referanceId, string schema, string connectionString);
    
    bool Delete (Guid id, string schema, string connectionString);
  }
}