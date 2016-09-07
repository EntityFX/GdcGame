using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class GameDataContractMapper : IMapper<IGame, GameData>
    {
        private readonly IMapper<GameCash, Cash> _fundsCountersContractMapper;
        private readonly IMapper<Item, Common.Contract.Items.Item> _fundsDriversContractMapper;

        public GameDataContractMapper(
            IMapper<GameCash, Cash> fundsCountersContractMapper,
            IMapper<Item, Common.Contract.Items.Item> fundsDriversContractMapper
            )
        {
            _fundsCountersContractMapper = fundsCountersContractMapper;
            _fundsDriversContractMapper = fundsDriversContractMapper;
        }

        public GameData Map(IGame source, GameData destination = null)
        {
            return new GameData
            {
                Items = source.Items.Select(fundsDriver =>
                {
                    var destinationFundDriver = _fundsDriversContractMapper.Map((fundsDriver.Value));
                    destinationFundDriver.IsUnlocked = destinationFundDriver.UnlockBalance <=
                                                     source.GameCash.RootCounter.Value;
                    return destinationFundDriver;
                }).ToArray(),
                Cash = _fundsCountersContractMapper.Map(source.GameCash),
                AutomatedStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber
            };
        }
    }
}