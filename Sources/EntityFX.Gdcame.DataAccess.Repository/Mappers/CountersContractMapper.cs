using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class CountersContractMapper : IMapper<CounterEntity, CounterBase>
    {
        private static readonly Func<CounterEntity, GenericCounter> GenericFunc = source => new GenericCounter
        {
            UseInAutoSteps = source.UseInAutostep,
            InflationIncreaseSteps = source.InflationIncreaseSteps,
            Inflation = 0,
            Bonus = 0,
            BonusPercentage = 0,
            SubValue = source.InitialValue
        };

        private static readonly IDictionary<int, Func<CounterEntity, CounterBase>> MappingDictionary =
            new ReadOnlyDictionary<int, Func<CounterEntity, CounterBase>>(new Dictionary<int, Func<CounterEntity, CounterBase>>(new Dictionary<int, Func<CounterEntity, CounterBase>>()
            {
                {
                    0, entity => new SingleCounter()
                },
                {
                    1, GenericFunc
                },
                {
                    2, entity => new DelayedCounter
                    {
                        MiningTimeSeconds = entity.MiningTimeSeconds ?? 0,
                        SecondsRemaining = 0,
                        UnlockValue = 10000
                    }
                }
            }));

        public CounterBase Map(CounterEntity source, CounterBase destination = null)
        {
            destination = MappingDictionary[source.Type](source);
            if (destination == null) return null;
            destination.Name = source.Name;
            destination.Value = source.InitialValue;
            destination.Type = source.Type;
            destination.Id = source.Id;
            return destination;
        }
    }
}