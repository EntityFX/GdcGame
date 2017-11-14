using System;
using NLog;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure
{
    public class NLoggerAdapter : Infrastructure.Common.ILogger
    {
        private readonly NLog.ILogger _nLogLogger;

        public NLoggerAdapter(NLog.ILogger nLogLogger)
        {
            _nLogLogger = nLogLogger;
        }

        public void Debug(string message, params object[] args)
        {
            _nLogLogger.Debug(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            _nLogLogger.Trace(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _nLogLogger.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _nLogLogger.Warn(message, args);
        }

        public void Error(Exception exception)
        {
            _nLogLogger.Error(exception);
        }
    }
}