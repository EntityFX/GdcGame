using System.Linq;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreFundsCountersContractMapper : IMapper<FundsCounters, StoreFundsCounters>
    {
        private readonly IMapper<CounterBase, StoreCounterBase> _counterContractMapper;

        public StoreFundsCountersContractMapper(IMapper<CounterBase, StoreCounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public StoreFundsCounters Map(FundsCounters source, StoreFundsCounters destination = null)
        {
            return new StoreFundsCounters()
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter.Value)).ToArray(),
                CurrentFunds = source.CurrentFunds,
                TotalFunds = source.TotalFunds
            };
        }
    }
}