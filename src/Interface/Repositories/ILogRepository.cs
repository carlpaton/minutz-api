using System.Collections.Generic;

namespace Interface.Repositories
{
  public interface ILogRepository
  {
    bool Log(string schema,
             string connectionString,
             int logId,
             string logLevel,
             string log);
    List<Models.Entities.EventLog> Logs(string schema,
                                        string connectionString);
  }
}
