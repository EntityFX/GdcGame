using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{

    public interface ILogger
    {
        void Debug(string message, params object[] args);
        void Trace(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(Exception exception);
    }
}