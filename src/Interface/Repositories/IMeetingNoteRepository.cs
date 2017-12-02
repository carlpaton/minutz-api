using System;
using System.Collections.Generic;
using Models.Entities;

namespace Interface.Repositories
{
  public interface IMeetingNoteRepository
  {
    MeetingNote Get(Guid id, string schema, string connectionString);
    List<MeetingNote> GetMeetingNotes(Guid referenceId, string schema, string connectionString);
    IEnumerable<MeetingNote> List(string schema, string connectionString);
    bool Add(MeetingNote action, string schema, string connectionString);
    bool Update(MeetingNote action, string schema, string connectionString);
    bool DeleteMeetingNotes(Guid referenceId, string schema, string connectionString);
    bool Delete(Guid id, string schema, string connectionString);
  }
}
