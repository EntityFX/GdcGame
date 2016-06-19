using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class GameDataContractMapper : IMapper<IGame, GameData>
    {
        private readonly IMapper<FundsCounters, Contract.Manager.GameManager.Counters.FundsCounters> _fundsCountersContractMapper;
        private readonly IMapper<FundsDriver, Contract.Manager.GameManager.Funds.FundsDriver> _fundsDriversContractMapper;

        public GameDataContractMapper(
            IMapper<FundsCounters, Contract.Manager.GameManager.Counters.FundsCounters> fundsCountersContractMapper,
                        IMapper<FundsDriver, Contract.Manager.GameManager.Funds.FundsDriver> fundsDriversContractMapper
            )
        {
            _fundsCountersContractMapper = fundsCountersContractMapper;
            _fundsDriversContractMapper = fundsDriversContractMapper;
        }

        public GameData Map(IGame source)
        {
            return new GameData()
            {
                FundsDrivers = source.FundsDrivers.Select(fundsDriver => _fundsDriversContractMapper.Map((fundsDriver.Value))).ToArray(),
                Counters = _fundsCountersContractMapper.Map(source.FundsCounters)
            };
        }
    }
}