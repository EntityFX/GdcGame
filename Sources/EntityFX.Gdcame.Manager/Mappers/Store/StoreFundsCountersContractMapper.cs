using System.Linq;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    public class StoreFundsCountersContractMapper : IMapper<GameCash, StoredCash>
    {
        private readonly IMapper<CounterBase, StoredCounterBase> _counterContractMapper;

        public StoreFundsCountersContractMapper(IMapper<CounterBase, StoredCounterBase> counterContractMapper)
        {
            _counterContractMapper = counterContractMapper;
        }

        public StoredCash Map(GameCash source, StoredCash destination = null)
        {
            return new StoredCash
            {
                Counters = source.Counters.Select(counter => _counterContractMapper.Map(counter)).ToArray(),
                Balance = source.CashOnHand,
                TotalEarned = source.TotalEarned
            };
        }
    }
}