using System.Linq;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using CounterBase = EntityFX.Gdcame.GameEngine.Contract.Counters.CounterBase;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class FundsCountersContractMapper : IMapper<GameCash, Cash>
    {
        private readonly IMapper<CounterBase, Common.Contract.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(
            IMapper<CounterBase, Common.Contract.Counters.CounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public Cash Map(GameCash source, Cash destination = null)
        {
            return new Cash
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter)).ToArray(),
                OnHand = source.CashOnHand,
                Total = source.TotalEarned
            };
        }
    }
}