using System.Collections.Generic;
using Models.Entities;
using System;

namespace Interface.Repositories
{
  public interface IMeetingAttendeeRepository
  {
    MeetingAttendee Get(Guid id, string schema, string connectionString);
    List<MeetingAttendee> GetMeetingAttendees(Guid referenceId, string schema, string connectionString);
    List<MeetingAttendee> GetAvalibleAttendees(string schema, string connectionString);
    IEnumerable<MeetingAttendee> List(string schema, string connectionString);
    bool Add(MeetingAttendee action, string schema, string connectionString);
    bool Update(MeetingAttendee action, string schema, string connectionString);
    bool Delete(Guid id, string schema, string connectionString);
  }
}
