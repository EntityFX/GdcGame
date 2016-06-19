using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class CounterContractMapper : IMapper<CounterBase, Contract.Manager.GameManager.Counters.CounterBase>
    {
        public Contract.Manager.GameManager.Counters.CounterBase Map(CounterBase source)
        {
            Contract.Manager.GameManager.Counters.CounterBase destinationCounter = null;
            var genericCounter = source as GenericCounter;
            if (genericCounter != null)
            {
                destinationCounter = new Contract.Manager.GameManager.Counters.GenericCounter();
            }

            var singleCounter = source as SingleCounter;
            if (singleCounter != null)
            {
                destinationCounter = new Contract.Manager.GameManager.Counters.SingleCounter();
            }
            if (destinationCounter != null)
            {
                MapCommon(source, destinationCounter);
            }

            return  destinationCounter;
        }

        private void MapCommon(CounterBase source, Contract.Manager.GameManager.Counters.CounterBase destination)
        {
            destination.Name = source.Name;
            destination.Value = source.Value;
        }
    }
}