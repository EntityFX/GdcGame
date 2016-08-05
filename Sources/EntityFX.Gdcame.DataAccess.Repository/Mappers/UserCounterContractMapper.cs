using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class UserCounterContractMapper : IMapper<UserCounterEntity, CounterBase>
    {
        private static readonly Func<UserCounterEntity, GenericCounter> GenericFunc = source => new GenericCounter
        {
            UseInAutoSteps = source.Counter.UseInAutostep,
            InflationIncreaseSteps = source.Counter.InflationIncreaseSteps,
            Inflation = source.Inflation,
            BonusPercentage = source.BonusPercentage,
            CurrentSteps = source.CurrentStepsCount,
            SubValue = source.Value
        };

        private static readonly IDictionary<int, Func<UserCounterEntity, CounterBase>> MappingDictionary =
    new ReadOnlyDictionary<int, Func<UserCounterEntity, CounterBase>>(new Dictionary<int, Func<UserCounterEntity, CounterBase>>(new Dictionary<int, Func<UserCounterEntity, CounterBase>>()
            {
                {
                    0, entity => new SingleCounter 
                    {
                        Value =entity.Value
                    }
                },
                {
                    1, GenericFunc
                },
                {
                    2, entity => new DelayedCounter
                    {
                        SecondsRemaining = entity.MiningTimeSecondsEllapsed,
                        UnlockValue = 10000,
                        MiningTimeSeconds = entity.Counter.MiningTimeSeconds ?? 0,
                         Value =entity.Value
                    }
                }
            }));

        public CounterBase Map(UserCounterEntity source, CounterBase destination = null)
        {
            destination = MappingDictionary[source.Counter.Type](source);
            if (destination == null) return null;
            destination.Name = source.Counter.Name;
            destination.Type = source.Counter.Type;
            destination.Id = source.Counter.Id;
            return destination;
        }
    }
}