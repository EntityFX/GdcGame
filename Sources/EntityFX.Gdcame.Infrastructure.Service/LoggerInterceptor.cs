using System;
using System.Collections.Generic;
using EntityFX.Gdcame.Infrastructure.Common;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    public class LoggerInterceptor : IInterceptionBehavior
    {
        private readonly ILogger _logger;

        public LoggerInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {

            _logger.Debug("Interecptor: {0}, Call begin {1}.{2}", GetType(), input.Target, input.MethodBase.Name);
            var result = getNext()(input, getNext);

            if (result.Exception != null)
            {
                _logger.Error(result.Exception);
            }
            else
            {
                _logger.Debug("Interecptor: {0}, Call end {1}.{2} [Return={3}]", GetType(), input.Target, input.MethodBase.Name, result.ReturnValue);
            }
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute { get { return true; } }
    }

    public class LoggerCallHandler : ICallHandler
    {
        private readonly ILogger _logger;

        public LoggerCallHandler(ILogger logger)
        {
            _logger = logger;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            _logger.Debug("Interecptor: {0}, Call begin {1}.{2}", GetType(), input.Target, input.MethodBase.Name);
            var result = getNext()(input, getNext);

            if (result.Exception != null)
            {
                _logger.Error(result.Exception);
            }
            else
            {
                _logger.Debug("Interecptor: {0}, Call end {1}.{2} [Return={3}]", GetType(), input.Target, input.MethodBase.Name, result.ReturnValue);
            }
            return result;
        }

        public int Order { get; set; }
    }
}
