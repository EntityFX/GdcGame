using System;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class UserCounterEntityMapper : IMapper<CounterBase, UserCounterEntity>
    {
        public UserCounterEntity Map(CounterBase source, UserCounterEntity destination = null)
        {
            destination = destination ?? new UserCounterEntity();
            switch (source.Type)
            {
                case 0:
                    var singleCounter = (SingleCounter)source;
                    destination.Value = singleCounter.Value;
                    break;
                case 1:
                    var genericCounter = (GenericCounter)source;
                    destination.BonusPercentage = genericCounter.BonusPercentage;
                    destination.CounterId = genericCounter.Id;
                    destination.CreateDateTime = DateTime.Now;
                    destination.Inflation = genericCounter.Inflation;
                    destination.CurrentStepsCount = genericCounter.CurrentSteps;
                    destination.Value = genericCounter.SubValue;
                    break;
                case 2:
                    var delayedCounter = (DelayedCounter)source;
                    destination.MiningTimeSecondsEllapsed = delayedCounter.SecondsRemaining;
                    destination.Value = delayedCounter.Value;
                    break;
            }
            destination.CounterId = source.Id;
            return destination;
        }
    }
}