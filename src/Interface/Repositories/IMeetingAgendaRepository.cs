using System.Collections.Generic;
using Minutz.Models.Entities;
using System;

namespace Interface.Repositories
{
  public interface IMeetingAgendaRepository
  {
    bool Update(MeetingAgenda action, string schema, string connectionString);
    bool Add(MeetingAgenda action, string schema, string connectionString);
    IEnumerable<MeetingAgenda> List(string schema, string connectionString);
    MeetingAgenda Get(Guid id, string schema, string connectionString);
    List<MeetingAgenda> GetMeetingAgenda(Guid referenceId, string schema, string connectionString);
    bool DeleteMeetingAgenda(Guid referenceId, string schema, string connectionString);
    bool Delete(Guid id, string schema, string connectionString);
  }
}
