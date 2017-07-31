using System.Linq;

using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class GameDataContractMapper : IMapper<IGame, GameData>
    {
        private readonly IMapper<GameCash, Cash> _fundsCountersContractMapper;
        private readonly IMapper<Item, Gdcame.Contract.MainServer.Items.Item> _fundsDriversContractMapper;

        public GameDataContractMapper(
            IMapper<GameCash, Cash> fundsCountersContractMapper,
            IMapper<Item, Gdcame.Contract.MainServer.Items.Item> fundsDriversContractMapper
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
                    var destinationFundDriver = _fundsDriversContractMapper.Map((fundsDriver));
                    destinationFundDriver.IsUnlocked = destinationFundDriver.UnlockValue <=
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