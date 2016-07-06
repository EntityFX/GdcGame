using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class FundsCountersContractMapper : IMapper<FundsCounters, Contract.Common.Counters.FundsCounters>
    {
        private readonly IMapper<CounterBase, Contract.Common.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(IMapper<CounterBase, Contract.Common.Counters.CounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public Contract.Common.Counters.FundsCounters Map(FundsCounters source, Contract.Common.Counters.FundsCounters destination = null)
        {
            return new Contract.Common.Counters.FundsCounters()
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter.Value)).ToArray(),
                CurrentFunds = source.CurrentFunds,
                TotalFunds = source.TotalFunds
            };
        }
    }
}