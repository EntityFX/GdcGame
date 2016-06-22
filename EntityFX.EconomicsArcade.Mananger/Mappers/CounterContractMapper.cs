﻿using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class CounterContractMapper : IMapper<CounterBase, Contract.Common.Counters.CounterBase>
    {
        public Contract.Common.Counters.CounterBase Map(CounterBase source, Contract.Common.Counters.CounterBase destination)
        {
            Contract.Common.Counters.CounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                var genericDestination = new Contract.Common.Counters.GenericCounter
                {
                    Bonus = genericCounter.Bonus,
                    BonusPercentage = genericCounter.BonusPercentage,
                    SubValue = genericCounter.SubValue
                };
                destinationCounter = genericDestination;
                destinationCounter.Value = genericCounter.Value;
            }

            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destinationCounter = new Contract.Common.Counters.SingleCounter {Value = singleCounter.Value};
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            return  destinationCounter;
        }

        private void MapCommon(CounterBase source, Contract.Common.Counters.CounterBase destination)
        {
            destination.Name = source.Name;
        }
    }
}