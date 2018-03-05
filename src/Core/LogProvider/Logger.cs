using System;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Logging;
using SqlRepository;

namespace Core.LogProvider {
    public class Logger : ILogger {
        private readonly string _categoryName;
        private readonly ILogRepository _repository;
        private readonly IApplicationSetting _appSettings;
        public Logger (string categoryName) {
            this._categoryName = categoryName;
            this._repository = new LogRepository ();
            this._appSettings = new ApplicationSetting (new InstanceRepository());
        }
        IDisposable ILogger.BeginScope<TState> (TState state) {
            return new LogDisposable ();
        }

        bool ILogger.IsEnabled (LogLevel logLevel) {
            return true;
        }

        internal void RecordMsg<TState> (
            LogLevel logLevel,
            EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter) {
            _repository.Log (this._appSettings.Schema,
            this._appSettings.CreateConnectionString (),
                eventId.Id, Enum.GetName (typeof (LogLevel), logLevel), formatter (state, exception)
            );
        }
        void ILogger.Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (logLevel == LogLevel.Critical ||
                logLevel == LogLevel.Error ||
                logLevel == LogLevel.Warning ||
                logLevel == LogLevel.Information) {
                RecordMsg (logLevel, eventId, state, exception, formatter);
            }
        }
    }
    public class LogDisposable : IDisposable {
        public void Dispose () { }
    }
}