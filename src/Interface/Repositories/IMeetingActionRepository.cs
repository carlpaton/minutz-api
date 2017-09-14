using System.Collections.Generic;
using Models.Entities;
using System;

namespace Interface.Repositories
{
    public interface IMeetingActionRepository
    {
      MeetingAction Get(Guid id, string schema, string connectionString);
      IEnumerable<MeetingAction> List(string schema, string connectionString);
      bool Add(MeetingAction action, string schema, string connectionString);
      bool Update(MeetingAction action, string schema, string connectionString);
    }
}
