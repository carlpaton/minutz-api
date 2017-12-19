namespace Interface.Repositories
{
  public interface ILogRepository
  {
    bool Log(string schema,
             string connectionString,
             int logId,
             string logLevel,
             string log);
  }
}
