using System.Linq;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class FundsCountersContractMapper : IMapper<FundsCounters, Gdcame.Common.Contract.Counters.FundsCounters>
    {
        private readonly IMapper<CounterBase, Gdcame.Common.Contract.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(IMapper<CounterBase, Gdcame.Common.Contract.Counters.CounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public Gdcame.Common.Contract.Counters.FundsCounters Map(FundsCounters source, Gdcame.Common.Contract.Counters.FundsCounters destination = null)
        {
            return new Gdcame.Common.Contract.Counters.FundsCounters()
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter.Value)).ToArray(),
                CurrentFunds = source.CurrentFunds,
                TotalFunds = source.TotalFunds
            };
        }
    }
}