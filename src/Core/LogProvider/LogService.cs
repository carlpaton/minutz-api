using System;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models;

namespace Core.LogProvider
{
  public class LogService : ILogService
  {
    private readonly ILogRepository _logRepository;
    private readonly IApplicationSetting _applicationSetting;

    public LogService(ILogRepository logRepository,
                      IApplicationSetting applicationSetting)
    {
      this._logRepository = logRepository;
      this._applicationSetting = applicationSetting;
    }

    public void Log(LogLevel level,
                    string log)
    {
      if (string.IsNullOrEmpty(log))
        throw new ArgumentException("No log message was supplied.");
      try
      {
        var connectionString = _applicationSetting.CreateConnectionString(
                                                   _applicationSetting.Server,
                                                   _applicationSetting.Catalogue,
                                                   _applicationSetting.Username,
                                                   _applicationSetting.Password);
        if (string.IsNullOrEmpty(connectionString))
          throw new ArgumentNullException(nameof(connectionString),
                                          "The connection string was not provided in the log service.");

        var result = this._logRepository.Log(this._applicationSetting.Schema,
                                             connectionString,
                                             (int)level,
                                             Enum.GetName(typeof(LogLevel), level),
                                             log);
      }
      catch (Exception ex)
      {
        throw new Exception("Log Service found a issue.", ex.InnerException);
      }

    }
  }
}
