using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreGameDataContractMapper : IMapper<IGame, StoreGameData>
    {
        private readonly IMapper<FundsCounters, StoreFundsCounters> _fundsCountersContractMapper;
        private readonly IMapper<FundsDriver, StoreFundsDriver> _fundsDriversContractMapper;

        public StoreGameDataContractMapper(
            IMapper<FundsCounters, StoreFundsCounters> fundsCountersContractMapper,
                        IMapper<FundsDriver, StoreFundsDriver> fundsDriversContractMapper
            )
        {
            _fundsCountersContractMapper = fundsCountersContractMapper;
            _fundsDriversContractMapper = fundsDriversContractMapper;
        }

        public StoreGameData Map(IGame source, StoreGameData destination = null)
        {
            return new StoreGameData()
            {
                FundsDrivers = source.FundsDrivers.Select(fundsDriver =>
                {
                    var destinationFundDriver = _fundsDriversContractMapper.Map((fundsDriver.Value));
                    return destinationFundDriver;
                }).ToArray(),
                Counters = _fundsCountersContractMapper.Map(source.FundsCounters),
                AutomaticStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber
            };
        }

    }
}