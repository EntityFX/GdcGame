using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public class Logger : ILogger
    {
        ILogger _loggerAdapter;

        public Logger(ILogger adapter)
        {
            _loggerAdapter = adapter;
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
