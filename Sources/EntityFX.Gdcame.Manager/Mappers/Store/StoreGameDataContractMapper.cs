using System.Linq;

using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;

    using EntityFX.Gdcame.Kernel.Contract;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class StoreGameDataContractMapper : IMapper<IGame, StoredGameData>
    {
        private readonly IMapper<GameCash, StoredCash> _fundsCountersContractMapper;
        private readonly IMapper<Item, StoredItem> _fundsDriversContractMapper;

        public StoreGameDataContractMapper(
            IMapper<GameCash, StoredCash> fundsCountersContractMapper,
            IMapper<Item, StoredItem> fundsDriversContractMapper
            )
        {
            _fundsCountersContractMapper = fundsCountersContractMapper;
            _fundsDriversContractMapper = fundsDriversContractMapper;
        }

        public StoredGameData Map(IGame source, StoredGameData destination = null)
        {
            return new StoredGameData
            {
                Items = source.Items.Select(fundsDriver =>
                {
                    var destinationFundDriver = _fundsDriversContractMapper.Map((fundsDriver));
                    return destinationFundDriver;
                }).ToArray(),
                Cash = _fundsCountersContractMapper.Map(source.GameCash),
                AutomatedStepsCount = source.AutomaticStepNumber,
                ManualStepsCount = source.ManualStepNumber
            };
        }
    }
}