using Minutz.Models;

namespace Interface.Services
{
  public interface ILogService
  {
    void Log(LogLevel level, string log);
  }
}
