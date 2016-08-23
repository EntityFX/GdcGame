using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Presentation.Model.Mappers
{
    public class GameDataModelMapper : IMapper<GameData, GameDataModel>
    {
        private readonly IMapper<Cash, FundsCounterModel> _fundsCounterModelMapper;
        private readonly IMapper<Item, FundsDriverModel> _fundsDriverModelMapper;

        public GameDataModelMapper(
            IMapper<Item, FundsDriverModel> fundsDriverModelMapper
            , IMapper<Cash, FundsCounterModel> fundsCounterModelMapper)
        {
            _fundsDriverModelMapper = fundsDriverModelMapper;
            _fundsCounterModelMapper = fundsCounterModelMapper;
        }

        public GameDataModel Map(GameData source, GameDataModel destination = null)
        {
            destination = destination ?? new GameDataModel();
            var fundsDriverModels = new List<FundsDriverModel>();
            foreach (var fundsDriver in source.Items)
            {
                fundsDriverModels.Add(_fundsDriverModelMapper.Map(fundsDriver));
            }
            destination.FundsDrivers = fundsDriverModels.ToArray();
            destination.Counters = _fundsCounterModelMapper.Map(source.Cash);

            return destination;
        }
    }
}