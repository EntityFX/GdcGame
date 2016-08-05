using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class GameDataContractMapper : IMapper<IGame, GameData>
    {
        private readonly IMapper<FundsCounters, Gdcame.Common.Contract.Counters.FundsCounters> _fundsCountersContractMapper;
        private readonly IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver> _fundsDriversContractMapper;

        public GameDataContractMapper(
            IMapper<FundsCounters, Gdcame.Common.Contract.Counters.FundsCounters> fundsCountersContractMapper,
                        IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver> fundsDriversContractMapper
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