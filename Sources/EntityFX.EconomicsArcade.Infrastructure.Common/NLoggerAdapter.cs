﻿using System;
using PortableLog.Core;


namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public class NLoggerAdapter : ILogger
    {
        ILog _nLogLogger;

        public NLoggerAdapter(ILog nLogLogger)
        {
            _nLogLogger = nLogLogger;
        }

        public void Trace(string message, params object[] args)
        {
            _nLogLogger.TraceFormat(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _nLogLogger.InfoFormat(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _nLogLogger.WarnFormat(message, args);
        }

        public void Error(Exception exception)
        {
            _nLogLogger.Error(exception);
        }
    }
}
