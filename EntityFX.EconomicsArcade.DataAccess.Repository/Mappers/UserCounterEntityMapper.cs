using System;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
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
                    break;
                case 1:
                    break;
                case 2:
                    var genericCounter = (GenericCounter)source;
                    destination.Bonus = genericCounter.Bonus;
                    destination.BonusPercentage = genericCounter.BonusPercentage;
                    destination.CounterId = genericCounter.Id;
                    destination.CreateDateTime = DateTime.Now;
                    destination.Value = genericCounter.Value;
                    destination.Inflation = genericCounter.Inflation;
                    break;
                case 3:
                    var delayedCounter = (DelayedCounter)source;
                    break;
            }
            return destination;
        }
    }
}