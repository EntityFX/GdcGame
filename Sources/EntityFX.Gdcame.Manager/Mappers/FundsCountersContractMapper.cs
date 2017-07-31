using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Counters;

    using CounterBase = EntityFX.Gdcame.Kernel.Contract.Counters.CounterBase;

    public class FundsCountersContractMapper : IMapper<GameCash, Cash>
    {
        private readonly IMapper<CounterBase, Gdcame.Contract.MainServer.Counters.CounterBase> _counterContractMapper;

        public FundsCountersContractMapper(
            IMapper<CounterBase, Gdcame.Contract.MainServer.Counters.CounterBase> counterContractMapper)
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