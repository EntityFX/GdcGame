using System.Linq;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class FundsCountersContractMapper : IMapper<GameCash, Gdcame.Common.Contract.Counters.Cash>
    {
        private readonly IMapper<CounterBase, Gdcame.Common.Contract.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(IMapper<CounterBase, Gdcame.Common.Contract.Counters.CounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public Gdcame.Common.Contract.Counters.Cash Map(GameCash source, Gdcame.Common.Contract.Counters.Cash destination = null)
        {
            return new Gdcame.Common.Contract.Counters.Cash()
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter.Value)).ToArray(),
                CashOnHand = source.CashOnHand,
                TotalEarned = source.TotalEarned
            };
        }
    }
}