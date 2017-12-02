using System.Collections.Generic;
using Models.Entities;
using System;

namespace Interface.Repositories
{
  public interface IMeetingActionRepository
  {
    MinutzAction Get(Guid id, string schema, string connectionString);
    List<MinutzAction> GetMeetingActions(Guid referenceId, string schema, string connectionString);
    IEnumerable<MinutzAction> List(string schema, string connectionString);
    bool Add(MinutzAction action, string schema, string connectionString);
    bool Update(MinutzAction action, string schema, string connectionString);
    bool DeleteMeetingActions(Guid referenceId, string schema, string connectionString);
    bool Delete(Guid id, string schema, string connectionString);
  }
}
