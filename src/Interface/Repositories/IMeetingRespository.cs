using System.Collections.Generic;
using Models.Entities;
using System;

namespace Interface.Repositories
{
  public interface IMeetingRespository
  {
    Meeting Get(Guid id, string schema, string connectionString);
    IEnumerable<Meeting> List(string schema, string connectionString);
    bool Add(Meeting action, string schema, string connectionString);
    bool Update(Meeting action, string schema, string connectionString);
  }
}
