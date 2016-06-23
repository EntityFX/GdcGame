using System;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{

    public interface ILogger
    {
        void Trace(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(Exception exception);
    }
}