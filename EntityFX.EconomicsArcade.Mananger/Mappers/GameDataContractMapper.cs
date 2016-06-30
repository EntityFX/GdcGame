using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class GameDataContractMapper : IMapper<IGame, GameData>
    {
        private readonly IMapper<FundsCounters, Contract.Common.Counters.FundsCounters> _fundsCountersContractMapper;
        private readonly IMapper<FundsDriver, Contract.Common.Funds.FundsDriver> _fundsDriversContractMapper;

        public GameDataContractMapper(
            IMapper<FundsCounters, Contract.Common.Counters.FundsCounters> fundsCountersContractMapper,
                        IMapper<FundsDriver, Contract.Common.Funds.FundsDriver> fundsDriversContractMapper
            )
        {
            _fundsCountersContractMapper = fundsCountersContractMapper;
            _fundsDriversContractMapper = fundsDriversContractMapper;
        }

        public GameData Map(IGame source, GameData destination = null)
        {
            return new GameData()
            {
                FundsDrivers = source.FundsDrivers.Select(fundsDriver =>
                {
                    var destinationFundDriver = _fundsDriversContractMapper.Map((fundsDriver.Value));
                    destinationFundDriver.IsActive = destinationFundDriver.UnlockValue <=
                                                     source.FundsCounters.RootCounter.Value;
                    return destinationFundDriver;
                }).ToArray(),
                Counters = _fundsCountersContractMapper.Map(source.FundsCounters),
                AutomaticStepsCount = source.AutomaticStepNumber,
                ManualStepsCount =  source.ManualStepNumber
            };
        }
    }
}