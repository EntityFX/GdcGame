using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface ITaskTimerFactory
    {
        ITaskTimer Build(TimeSpan interval, Action tick, bool runOnce = false);
    }
}