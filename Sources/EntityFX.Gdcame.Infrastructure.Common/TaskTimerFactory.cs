using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class TaskTimerFactory : ITaskTimerFactory
    {
        private readonly IResolver _unityContainer;

        public TaskTimerFactory(IResolver unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public ITaskTimer Build(TimeSpan interval, Action tick, bool runOnce = false)
        {
            return _unityContainer.Resolve<ITaskTimer>(null,
                new Tuple<string, object>("interval", interval), 
                new Tuple<string, object>("tick", tick), 
                new Tuple<string, object>("runOnce", runOnce));
        }
    }
}