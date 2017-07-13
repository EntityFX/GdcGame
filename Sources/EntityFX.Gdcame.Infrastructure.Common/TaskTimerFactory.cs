using System;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class TaskTimerFactory : ITaskTimerFactory
    {
        private readonly IUnityContainer _unityContainer;

        public TaskTimerFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public ITaskTimer Build(TimeSpan interval, Action tick, bool runOnce = false)
        {
            return _unityContainer.Resolve<ITaskTimer>(
                new ParameterOverride("interval", interval), 
                new ParameterOverride("tick", tick), 
                new ParameterOverride("runOnce", runOnce));
        }
    }
}