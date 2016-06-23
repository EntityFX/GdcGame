using System;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{

    public interface ILogger
    {
        void Log(string message);
        void Error(Exception exception);
    }
}