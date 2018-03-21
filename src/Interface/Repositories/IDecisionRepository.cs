using System;
using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
  public interface IDecisionRepository
  {
    List<MinutzDecision> GetMeetingDecisions
      (Guid referenceId, string schema, string connectionString);

    MinutzDecision Get
      (Guid id, string schema, string connectionString);

    bool Add
      (MinutzDecision decision, string schema, string connectionString);

    bool Update
      (MinutzDecision decision, string schema, string connectionString);

    bool Delete
      (Guid id, string schema, string connectionString);
  }
}