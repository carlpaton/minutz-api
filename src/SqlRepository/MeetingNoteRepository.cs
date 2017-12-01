using System;
using System.Collections.Generic;
using Interface.Repositories;
using Models.Entities;

namespace SqlRepository
{
  public class MeetingNoteRepository : IMeetingNoteRepository
  {
    public MeetingNote Get(Guid id, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<MeetingNote> List(string schema, string connectionString)
    {
      throw new NotImplementedException();
    }

    public bool Add(MeetingNote action, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }

    public bool Update(MeetingNote action, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }

    public bool Delete(Guid attachmentId, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
  }
}
