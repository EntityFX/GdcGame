using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class Logger : ILogger
    {
        ILogger _loggerAdapter;

        public Logger(ILogger adapter)
        {
            _loggerAdapter = adapter;
        }

        public void Debug(string message, params object[] args)
        {
            _loggerAdapter.Debug(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            _loggerAdapter.Trace(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _loggerAdapter.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _loggerAdapter.Warning(message, args);
        }

        public void Error(Exception exception)
        {
            _loggerAdapter.Error(exception);
        }


    }
}
