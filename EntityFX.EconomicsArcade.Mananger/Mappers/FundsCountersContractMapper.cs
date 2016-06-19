using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class FundsCountersContractMapper : IMapper<FundsCounters, Contract.Manager.GameManager.Counters.FundsCounters>
    {
        private readonly IMapper<CounterBase, Contract.Manager.GameManager.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(IMapper<CounterBase, Contract.Manager.GameManager.Counters.CounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public Contract.Manager.GameManager.Counters.FundsCounters Map(FundsCounters source)
        {
            return new Contract.Manager.GameManager.Counters.FundsCounters()
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter.Value)).ToArray(),
                CurrentFunds = source.CurrentFunds,
                TotalFunds = source.TotalFunds
            };
        }
    }
}