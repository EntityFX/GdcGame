using System;
using System.Collections.Generic;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.EconomicsArcade.Infrastructure.Service.Logger
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

            _logger.Debug("Interecptor: {0}, Call begin {1}", GetType(), input.GetType().ToString(), input.MethodBase.ToString());
            var result = getNext()(input, getNext);

            if (result.Exception != null)
            {
                _logger.Error(result.Exception);
            }
            else
            {
                _logger.Debug("Interecptor: {0}, Call end {1} [Return={2}]", GetType(), input.MethodBase.ToString(), result.ReturnValue);
            }
            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute { get { return true; } }
    }
}
